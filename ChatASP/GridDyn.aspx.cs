using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ChatASP
{
    public partial class GridDyn : System.Web.UI.Page
    {
        string _connStr = ConfigurationManager.ConnectionStrings["smrConnectionString"].ConnectionString;
        string columnsName;
        //Hashtable htControls = new Hashtable();
        ChatASP.ChatHandler ch = new ChatHandler();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                try
                {
                    GetData();
                    BindTableColumns();

                    
                }
                catch (Exception ex )
                {
                }

            }
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
        private void BindTableColumns()

        {

            DataTable table = new DataTable();

            using (SqlConnection conn = new SqlConnection(_connStr))

            {
                

               // using (SqlCommand cmd = new SqlCommand("sp_columns", conn))
                using (SqlCommand cmd = new SqlCommand("select ltrim(value) as Column_name from STRING_SPLIT('" + columnsName + "', ',')", conn))
                {

                    //cmd.CommandType = CommandType.StoredProcedure;

                    //cmd.Parameters.AddWithValue("@table_name", "WorkSv");

                    // get the adapter object and attach the command object to it

                    using (SqlDataAdapter ad = new SqlDataAdapter(cmd))

                    {

                        // fire Fill method to fetch the data and fill into DataTable

                        ad.Fill(table);

                    }

                    chkFields.DataSource = table;


                    chkFields.DataBind();
                    foreach (ListItem item in chkFields.Items)
                    {

                        item.Selected = true;

                    }

                }

            }

        }

        private void GetData()

        {

           // DataTable table = new DataTable();
            List<strFilter> str = ch.FilterAD( tFilter.Text);
            
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(str);
            DataTable table = JsonConvert.DeserializeObject<DataTable>(json);
            columnsName = string.Join(", ", table.Columns.OfType<DataColumn>().Select(r => r.ColumnName.ToString()));

            GridView1.DataSource = table;
            GridView1.DataBind();

        }



        protected void ShowGrid(object sender, EventArgs e)

        {
 
            foreach (ListItem item in chkFields.Items)

            {
                if (!item.Selected && GetIndex(GridView1, item.Value) >= 0)
                { GridView1.Columns.RemoveAt(GetIndex(GridView1, item.Value)); }
                else
               if (item.Selected && GetIndex(GridView1, item.Value) < 0)
                
                {

                    BoundField b = new BoundField();

                    b.DataField = item.Value;

                    b.HeaderText = item.Value;

                    GridView1.Columns.Add(b);

                }

            }

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