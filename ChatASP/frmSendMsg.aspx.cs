using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.DirectoryServices;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ChatASP
{
    public partial class frmSendMsg : System.Web.UI.Page
    {
        public DataView dv;
        protected void Page_Load(object sender, EventArgs e)
        {
           // Button1_Click(null, null);
            // ReportViewer1.LocalReport.Refresh()
            //  ReportViewer1.LocalReport.Refresh();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            dv = new DataView();
            SqlDataAdapter sda = new SqlDataAdapter("select top 1 * from MySmr", ConfigurationManager.ConnectionStrings[2].ToString());
            sda.SelectCommand.CommandTimeout = 3000000;
            DataSet DS = new DataSet();
            DataTable dt = new DataTable();
            sda.Fill(DS); dv.Table = DS.Tables[0];
            //ReportDataSource ReportDataSource1 = new ReportDataSource();
            //ReportDataSource1.Value = dv;
        //  ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("smrDataSetKS2KS3", dv));
            //Microsoft.Reporting.WebForms.ReportParameter par;
            //par = new Microsoft.Reporting.WebForms.ReportParameter("ReportParameter1", "1232");
            //ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { par });

            //String domain = System.Web.HttpContext.Current.User.Identity.Name.Split('\\').First();
            ////string searchbase = ddlZaal.SelectedItem.Text; //This is a dropdownlist to select an OU
            //// DirectoryEntry entry = new DirectoryEntry("LDAP://OU=" + searchbase + ",OU=YourOU,OU=YourSubOU," + Variables.Domain + ""); //Variables.Domain is specified in the Web.config
            //DirectoryEntry entry = new DirectoryEntry($"LDAP://{domain}");
            //DirectorySearcher mySearcher = new DirectorySearcher(entry);
            //mySearcher.Filter = ("(objectClass=computer)");
            ////foreach (SearchResult result in mySearcher.FindAll())
            ////{
            ////DirectoryEntry directoryObject = result.GetDirectoryEntry();
            ////string computernaam = directoryObject.Properties["Name"].Value.ToString();
            //string computernaam = "KT2-966";
            //lstComputers.Items.Add(computernaam); //This is a listbox that shows the computernames. To each computer a message is sent.
            //string pingnaam = computernaam + ",DC=titan2,DC=ru"; //Might be necessary for connecting to the computes in the domain
            //string MessageToSend = txtMessageToSend.Text; //The text in this textbox will be the messagetext
            //Process process = new Process();
            //ProcessStartInfo psi = new ProcessStartInfo(@"C:\inetpub\wwwroot\PsExec.exe"); //Location of PsExec.exe on the webserver that hosts the web-application.
            //psi.UseShellExecute = false;
            //psi.RedirectStandardOutput = true;
            //psi.RedirectStandardError = true;
            //psi.RedirectStandardInput = true;
            //psi.WindowStyle = ProcessWindowStyle.Normal;
            //psi.CreateNoWindow = true;
            //psi.Arguments = @"-accepteula -s -i -u jane -p QQQ!!!111  \\" + pingnaam + " cmd /c msg.exe * " + MessageToSend;
            //process.StartInfo = psi;
            //try
            //{
            //    process.Start();
            //    process.Close();
            //}
            //catch (Exception ex)
            //{
            //    //process.StandardOutput.ReadToEnd();
            //    throw;
            //}

        }

        protected void ObjectDataSource1_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {

        }

        protected void ObjectDataSource2_Selecting1(object sender, ObjectDataSourceSelectingEventArgs e)
        {

        }

        protected void ObjectDataSource1_Selecting1(object sender, ObjectDataSourceSelectingEventArgs e)
        {

        }
    }
}
