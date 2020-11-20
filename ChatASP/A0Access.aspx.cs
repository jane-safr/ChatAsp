using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ChatASP
{
    public partial class A0Access : System.Web.UI.Page
    {
        string _connStr = ConfigurationManager.ConnectionStrings["smrConnectionString"].ConnectionString;
 
        ChatASP.ChatHandler ch = new ChatHandler();
        protected void Page_Load(object sender, EventArgs e)
        {
            ChatASP.ChatHandler ch = new ChatHandler();
    
            welcome.Text = "Добро пожаловать, " + JObject.Parse(ch.dispayName(null))["user"]["displayName"] + "!";
        }
        private void ExportGridToExcel(GridView gv)
        {

            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            //Response.Charset = "";
            string FileName = "Vithal" + DateTime.Now + ".xls";
            StringWriter strwritter = new StringWriter();
            HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            Response.Charset = "utf-8";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            gv.GridLines = GridLines.Both;

            gv.HeaderStyle.Font.Bold = true;

            gv.RenderControl(htmltextwrtter);
            Response.Write(strwritter.ToString());
            Response.End();

        }

        private void GetData()

        {

            DataTable table = new DataTable();

            using (SqlConnection conn = new SqlConnection(_connStr))

            {

                // write the sql statement to execute

                string sql = @"SELECT        A0Data.dbo.A0User.Login, A0Data.dbo.A0User.Comment as Комментарий, A0Data.dbo.A0Role.RoleName as Роль
FROM A0Data.dbo.A0User INNER JOIN
                         A0Data.dbo.A0UserRole ON A0Data.dbo.A0User.UserID = A0Data.dbo.A0UserRole.UserID INNER JOIN
                         A0Data.dbo.A0Role ON A0Data.dbo.A0UserRole.RoleID = A0Data.dbo.A0Role.RoleID where A0Data.dbo.A0User.Login+ A0Data.dbo.A0User.Comment like '%" + tFilter.Text + "%' ORDER By  A0Data.dbo.A0User.Comment, A0Data.dbo.A0User.Login";

                // instantiate the command object to fire

                using (SqlCommand cmd = new SqlCommand(sql, conn))

                {

                    // get the adapter object and attach the command object to it

                    using (SqlDataAdapter ad = new SqlDataAdapter(cmd))

                    {

                        // fire Fill method to fetch the data and fill into DataTable

                        ad.Fill(table);
                    }

                }

            }
            if (GridView1.Columns.Count == 0)
            {
                foreach (DataColumn col in table.Columns)
                {

                    BoundField b = new BoundField();

                    b.DataField = col.ColumnName;

                    b.HeaderText = col.ColumnName;

                    GridView1.Columns.Add(b);
                }
            }
            GridView1.DataSource = table;
            GridView1.DataBind();

        }



        protected void ShowGrid(object sender, EventArgs e)

        {

            //foreach (ListItem item in chkFields.Items)

            //{
            //    if (!item.Selected && GetIndex(GridView1, item.Value) >= 0)
            //    { GridView1.Columns.RemoveAt(GetIndex(GridView1, item.Value)); }
            //    else
            //   if (item.Selected && GetIndex(GridView1, item.Value) < 0)

            //    {

            //        BoundField b = new BoundField();

            //        b.DataField = item.Value;

            //        b.HeaderText = item.Value;

            //        GridView1.Columns.Add(b);

            //    }

            //}


            this.GetData();

        }
        public static int GetIndex(GridView grd, string fieldName)
        {

            for (int i = 0; i < grd.Columns.Count; i++)
            {

                DataControlField field = grd.Columns[i];

                if (field != null && field.HeaderText == fieldName) return i;
            }

            return -1;
        }




        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            ExportGridToExcel(GridView1);
        }
    }
}