using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.WebSockets;
using System.DirectoryServices.Protocols;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices;
using System.Web.Security;
using System.Text;
using System.Web.Services;
using System.Web.Script.Services;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Diagnostics;
using System.Net.NetworkInformation;
//using Outlook = Microsoft.Office.Interop.Outlook;

namespace ChatASP
{
    /// <summary>
    /// Сводное описание для ChatHandler
    /// </summary>
    /// 

    public class user
    {
        public String displayName { get; set; }
        public String accountName { get; set; }
        public String ip { get; set; }

    }
    public class strFilter
    {
       // public String mode = "strFilter";
        public String displayName { get; set; }
        public String accountName { get; set; }
        public String extensionattribute3 { get; set; }
        public String memberOf { get; set; }

    }
    public class WS
    {
        public WebSocket client { get; set; }

        public user user { get; set; }
    }
    //Количество клиентов сокета
    public class userCount
    {
        public String mode = "usersCount";
        public int usersCount { get; set; }
    }
    public class ChatHandler : IHttpHandler
    {
        // Список всех клиентов
       //private static readonly List<WebSocket> Clients = new List<WebSocket>();
       private static readonly List<WS> Clients1 = new List<WS>();
       

        // Блокировка для обеспечения потокабезопасности
        private static readonly ReaderWriterLockSlim Locker = new ReaderWriterLockSlim();

        //Пользователь, открывший сокет. Серилизованная строка
        String userStr = "";
        //Подключение к PostgreSQL
        //PGLayer pg;

        ////Обновление пользователя
        //String userRef = "";

        public void ProcessRequest(HttpContext context)
        {
            if (context.IsWebSocketRequest)
                context.AcceptWebSocketRequest(WebSocketRequest);
        }

        private async Task WebSocketRequest(AspNetWebSocketContext context)
        {
            // Получаем сокет клиента из контекста запроса
            WebSocket socket = context.WebSocket;
            //Подключение к PostgreSQL
            PGLayer pg = new PGLayer();

            ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024]);



            // Добавляем его в список клиентов, проверяем пользователя
            Locker.EnterWriteLock();
            try
            {
                WS ws = new WS();
                ws.client = socket;
                //Проверяем пользователя
                userStr = dispayName(ws);
               // Clients1.RemoveAll(us => us.user.ip == ws.user.ip && us.user.accountName == ws.user.accountName);
                Clients1.Add(ws);
                //Зафиксировали подключение пользователя
                string err =pg.user_conn(ws.user.accountName, "подключился/лась");
                // pg.connect();
                //if (err =="")
                await socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(userStr)), WebSocketMessageType.Text, true, CancellationToken.None);
            }
            finally
            {
                Locker.ExitWriteLock();
            }
            //проверка через LDAP
            //if (!validateUserByBind("jane", ""))
            //{ return; }

            // Слушаем сокет
            while (true)
            {

                // Ожидаем данные от него
                var result = await socket.ReceiveAsync(buffer, CancellationToken.None);
                //Количество клиентов сокета
                userCount userCount = new userCount();

                //Передаём сообщение всем клиентам
                for (int i = 0; i < Clients1.Count; i++)
                {

                    WebSocket client = Clients1[i].client;

                    try
                    {
                       // Microsoft.Office.Interop.Outlook.Application oApp = new Microsoft.Office.Interop.Outlook.Application();
                        if (client.State == WebSocketState.Open)
                        {
                            var text = Encoding.UTF8.GetString(buffer.Array.ToArray(), 0, result.Count);
                            //  if (text.Trim() != "")
                            JObject ob = JObject.Parse(text);
                            //пользователь подключился к серверу
                            if (ob["mode"].ToString() == "join")
                            {
                                string err = pg.user_conn(ob["user"]["accountName"].ToString(), "отключился/лась");
                            }
                            else
                            //сообщение пользователям
                            if (ob["mode"].ToString() == "msg")
                            {
                                string err = pg.user_msg(ob["user"]["accountName"].ToString(), ob["msg"].ToString(), 1, Clients1[i].user.ip);
                                await client.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(text)), WebSocketMessageType.Text, true, CancellationToken.None);
                            }
                            else
                            //поиск пользователя в AD
                            if (ob["mode"].ToString() == "FindUser")
                            {
                                var strFilter1 = new { mode = "strFilter", accountName = Clients1[i].user.accountName, str = FilterAD( ob["strFilter"].ToString()) };
                                //string err = pg.user_msg(ob["user"]["accountName"].ToString(), ob["msg"].ToString(), 1, Clients1[i].user.ip);
                                await client.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(strFilter1))), WebSocketMessageType.Text, true, CancellationToken.None);
                            }


                            //  await client.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(JsonConvert.DeserializeObject(text)))), WebSocketMessageType.Text, true, CancellationToken.None);
                            //var userCount = new { mode = "usersCount", usersCount = Clients1.Count };
                            userCount.usersCount = Clients1.Count; //Clients1.Count(us => us.client.State == WebSocketState.Open);
                            await client.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(userCount))), WebSocketMessageType.Text, true, CancellationToken.None);
                        }
                    }

                    catch (ObjectDisposedException ex)
                    {
                        Locker.EnterWriteLock();
                        try
                        {
                            Clients1.RemoveAll(i1 => i1.client == client);
                            i--;
                        }
                        finally
                        {
                            Locker.ExitWriteLock();
                        }
                    }
                }
            }
        }


        public List<strFilter> FilterAD(string strFind)
        {
            //String displayName = "";
            String domain = System.Web.HttpContext.Current.User.Identity.Name.Split('\\').First();
            List<strFilter> strFilter1 = new List<strFilter>();
            strFilter strFilter = new strFilter();
            strFilter.accountName = "";
            strFilter.displayName = "";
            strFilter.extensionattribute3 = "";
            strFilter.memberOf = "";

            //String accountName = System.Web.HttpContext.Current.User.Identity.Name.Split('\\').Last();

            //accountName = "irina";
            // accountName = "KT2-966";
            using (var entry = new DirectoryEntry($"LDAP://{domain}"))
            {
                using (var searcher = new DirectorySearcher(entry))
                {
                    searcher.Filter = $"(|(extensionattribute3=*{strFind}*)(sAMAccountName={strFind})(displayName=*{strFind}*))";
                    searcher.PropertiesToLoad.Add("displayName");
                     searcher.PropertiesToLoad.Add("memberOf");
                     searcher.PropertiesToLoad.Add("samaccountname");
                    searcher.PropertiesToLoad.Add("extensionattribute3");
                    try
                    {


                        SearchResultCollection results = searcher.FindAll();
                        //int i = 0;
                        if (results.Count != 0)
                        {
                            foreach (SearchResult result in results)
                            {
                               strFilter = new strFilter();
                                strFilter.accountName = (result.Properties["samaccountname"].Count > 0 ? result.Properties["samaccountname"][0].ToString():"");
                                strFilter.displayName = (result.Properties["displayName"].Count > 0 ? result.Properties["displayName"][0].ToString() : "");
                                strFilter.extensionattribute3 = (result.Properties["extensionattribute3"].Count> 0?  result.Properties["extensionattribute3"][0].ToString():"");
                                strFilter.memberOf = (result.Properties["memberOf"].Count > 0 ? result.Properties["memberOf"][0].ToString() : "");
                                var searcher1 = new DirectorySearcher(entry);
                                searcher1.Filter = $"((extensionattribute3=*{strFilter.accountName}*))";
                                SearchResultCollection results1 = searcher1.FindAll();

                                foreach (SearchResult result1 in results1)
                                {
                                    strFilter.extensionattribute3 = strFilter.extensionattribute3 + "; " + result1.Properties["samaccountname"][0].ToString() +" " + result1.Properties["extensionattribute3"][0].ToString();
                                }
                               strFilter1.Add(strFilter);
                                Debug.WriteLine("/////////////////////////////////////////////");
                                //}
                                //i++;
                            }

                        }
                        else
                        {
                           // strFilter strFilter = new strFilter();
                            strFilter.accountName = "";
                            strFilter.displayName ="";
                            strFilter.extensionattribute3 = "";
                            strFilter.memberOf = "";
                       //     strFilter1.Add(strFilter);
                        }
                    }
                    catch (Exception ex)
                    {
                        
                        //  throw;
                    }
                }
            }
            if (strFilter1.Count == 0)
            strFilter1.Add(strFilter);
            return strFilter1;
        }
        public String dispayName(WS ws)
        {
            String displayName = "";
            String domain = System.Web.HttpContext.Current.User.Identity.Name.Split('\\').First();
            String accountName = System.Web.HttpContext.Current.User.Identity.Name.Split('\\').Last();

            //accountName = "irina";
           // accountName = "KT2-966";
            using (var entry = new DirectoryEntry($"LDAP://{domain}"))
            {
                using (var searcher = new DirectorySearcher(entry))
                {
                  //  searcher.Filter = $"(objectCategory=computer)";
                      searcher.Filter = $"(sAMAccountName={accountName})";
                      searcher.PropertiesToLoad.Add("displayName");
                    var searchResult = searcher.FindOne();
                    if (searchResult != null && searchResult.Properties.Contains("displayName"))
                    {
                        var user = new user();
                        user.displayName = searchResult.Properties["displayName"][0].ToString();
                        user.accountName = accountName;
                        user.ip = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();//GetIPAddress();
                        var wrapper = new {
                            mode = "userStart",
                            usersCount = Clients1.Count ,
                            msg = "подключился/лась " + user.displayName ,
                            user,
                            user.ip
                        };

                            //dispayName = wrapper;
                            displayName = JsonConvert.SerializeObject(wrapper);
                        if (ws != null)
                            ws.user = user;
                        }
                    else
                    {
                        // user not found
                    }
                }
            }
            return displayName;
        }
        public bool validateUserByBind(string username, string password)
        {
            bool result = false;

            try
            {
                using (PrincipalContext pc =
  new PrincipalContext(ContextType.Domain, "ad1.titan2.ru", "DC=titan2,DC=ru", "T2\\1c-ad", "Qwerty1234"))
                {
                    result = pc.ValidateCredentials(username, password);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                result = false;
            }
            return result;
        }

        protected string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;

            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}