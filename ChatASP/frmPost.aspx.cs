using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace ChatASP
{

    public partial class frmPost : System.Web.UI.Page
    {
        //PGLayer pg;
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
           // Microsoft.Office.Interop.Outlook.Application oApp = new Outlook.Application();
            //Outlook._Application _app = new Outlook.Application();
            //Outlook._NameSpace _ns = _app.GetNamespace("MAPI");
            //Outlook.MAPIFolder inbox = _ns.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderInbox);
            //_ns.SendAndReceive(true);
           // dt = new DataTable("Inbox");
            //System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage();
            //using (m)
            //{
            //    //sender is set in web.config:   <smtp from="my alias &lt;mymail@mysite.com&gt;">
            //    //m.To.Add(to);
            //    //if (!string.IsNullOrEmpty(cc))
            //    //    m.CC.Add(cc);
            //    //m.Subject = subject;
            //    //m.Body = body;
            //    //m.IsBodyHtml = isBodyHtml;
            //    //if (!string.IsNullOrEmpty(attachmentName))
            //    //    m.Attachments.Add(new System.Net.Mail.Attachment(attachmentFile, attachmentName));

            //    //System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            //    //try
            //    //{ client.Send(m); }
            //    //catch (System.Net.Mail.SmtpException) {/*errors can happen*/ }
            //}

            //Microsoft.Office.Interop.Outlook.Application oApp = new Microsoft.Office.Interop.Outlook.Application();
            //Microsoft.Office.Interop.Outlook.NameSpace oNS = oApp.GetNamespace("mapi");
            //oNS.Logon(null, null, true, true);

            //Microsoft.Office.Interop.Outlook.MailItem oMail = (Microsoft.Office.Interop.Outlook.MailItem)oApp.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);
            //oMail.Subject = "Sample Subject";
            //oMail.To = "test@yahoo.com";
            //oMail.HTMLBody = "Sample Paragraph";
            //oMail.Display(false);

            //oApp = null;
            //oNS = null;
            //oMail = null;
            //Outlook.Application application = null;


            //// If not, create a new instance of Outlook and sign in to the default profile.
            //application = new Outlook.Application();
            //Outlook.NameSpace nameSpace = application.GetNamespace("MAPI");
            //nameSpace.Logon("", "", Missing.Value, Missing.Value);
            //nameSpace = null;
            //// DisplayAccountInformation(application);
        }
    }
}
