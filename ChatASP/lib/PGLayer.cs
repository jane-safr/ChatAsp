using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using Devart.Data.PostgreSql;

namespace ChatASP
{
    public class PGLayer
    {
        PgSqlConnection connPG;
        PgSqlCommand cmdPG;
        PgSqlDataReader rdr;


        public string getMessage { get; set; }
        public PGLayer()
        {
            string cs = "User Id=postgres;Host=localhost;Database=mydb;Password =123;Persist Security Info=True;Initial Schema=public";
            connPG = new PgSqlConnection(cs);
            cmdPG = new PgSqlCommand();
            connPG.Charset = "UTF8";
        }

        public string user_conn(string accountName, string msg)
        {
            try
            {
                PGCmd(connPG, String.Format("insert into public.user_conn (login, connect) values('{0}','{1}') RETURNING id;", accountName, msg));
                return "";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return ex.Message;
            }
        }

        public string user_msg(string accountName, string msg, int to, string ip)
        {
            try
            {
                PGCmd(connPG, String.Format("select public.s_user_msg('{0}','{1}',{2},'{3}') ;", accountName, msg,to,ip));
                return "";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return ex.Message;
            }
        }

        public void PGCmd(PgSqlConnection conn, string insertStr)
        {
            conn.Open();
            PgSqlTransaction tx = conn.BeginTransaction(IsolationLevel.ReadCommitted);

            cmdPG.Connection = connPG;
            cmdPG.CommandText = insertStr;
            // PgSqlParameter parm = cmd.CreateParameter();
            //parm.ParameterName = "@name";
            //parm.Value = "SomeName";
            //cmd.Parameters.Add(parm);

            cmdPG.Prepare();
            try
            {
                cmdPG.ExecuteScalar();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            tx.Commit();
            conn.Close();
        }

        public void UseDataAdapter(PgSqlConnection pgConnection)
        {
            PgSqlDataAdapter myAdapter = new PgSqlDataAdapter("SELECT DeptNo, DName FROM Test.Dept", pgConnection);
            myAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            DataSet myDataSet = new DataSet();
            myAdapter.Fill(myDataSet, "Departments");
            object[] rowVals = new object[2];
            rowVals[0] = 40;
            rowVals[1] = "Operations";
            myDataSet.Tables["Departments"].Rows.Add(rowVals);
            myAdapter.InsertCommand = new PgSqlCommand("INSERT INTO Test.Dept (DeptNo, DName) " +
              "VALUES (:DeptNo, :DName)", pgConnection);
            myAdapter.InsertCommand.Parameters.Add("DeptNo", PgSqlType.Int, 0, "DeptNo");
            myAdapter.InsertCommand.Parameters.Add("DName", PgSqlType.VarChar, 15, "DName");
            myAdapter.Update(myDataSet, "Departments");
            //Get all data from all tables within the dataset
            foreach (DataTable myTable in myDataSet.Tables)
            {
                foreach (DataRow myRow in myTable.Rows)
                {
                    foreach (DataColumn myColumn in myTable.Columns)
                    {
                        Console.Write(myRow[myColumn] + "\t");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }
        public bool connect()
        {
            return GetRd("select * from users");
        }

        private bool GetRd(String strSQL)
        {
            int results = 0;
            int c;
            try
            {

                connPG.Open();
                cmdPG.CommandText = strSQL;// "select * from users";
                cmdPG.Connection = connPG;
                rdr = cmdPG.ExecuteReader();
                if (rdr == null)
                {

                    Console.WriteLine("IDataReader has a Null Reference.");
                }
                else
                {

                    do
                    {
                        // get the DataTable that holds
                        // the schema
                        DataTable dt = rdr.GetSchemaTable();
                        DataTable dtData = new DataTable();
                        DataView dv = new DataView();
                        dv.Table = dt;

                        if (rdr.RecordsAffected != -1)
                        {
                            // Results for 
                            // SQL INSERT, UPDATE, DELETE Commands 
                            // have RecordsAffected >= 0
                            System.Diagnostics.Debug.WriteLine("Result is from a SQL Command (INSERT,UPDATE,DELETE).  Records Affected: " + rdr.RecordsAffected);
                        }
                        else if (dt == null)
                            System.Diagnostics.Debug.WriteLine("Result is from a SQL Command not (INSERT,UPDATE,DELETE).   Records Affected: " + rdr.RecordsAffected);
                        else
                        {
                            // Results for
                            // SQL not INSERT, UPDATE, nor DELETE
                            // have RecordsAffected = -1
                            System.Diagnostics.Debug.WriteLine("Result is from a SQL SELECT Query.  Records Affected: " + rdr.RecordsAffected);

                            // Results for a SQL Command (CREATE TABLE, SET, etc)
                            // will have a null reference returned from GetSchemaTable()
                            // 
                            // Results for a SQL SELECT Query
                            // will have a DataTable returned from GetSchemaTable()

                            results++;
                            System.Diagnostics.Debug.WriteLine("Result Set " + results + "...");

                            // number of columns in the table
                            System.Diagnostics.Debug.WriteLine("   Total Columns: " +
                                dt.Columns.Count);

                            // display the schema
                            foreach (DataRow schemaRow in dt.Rows)
                            {
                                foreach (DataColumn schemaCol in dt.Columns)
                                    System.Diagnostics.Debug.WriteLine(schemaCol.ColumnName +
                                        " = " +
                                        schemaRow[schemaCol]);
                                // System.Diagnostics.Debug.WriteLine();
                            }

                            int nRows = 0;
                            string output, metadataValue, dataValue;
                            // Read and display the rows
                            System.Diagnostics.Debug.WriteLine("Gonna do a Read() now...");
                            while (rdr.Read())
                            {
                                System.Diagnostics.Debug.WriteLine("   Row " + nRows + ": ");

                                for (c = 0; c < rdr.FieldCount; c++)
                                {
                                    // column meta data 
                                    DataRow dr = dt.Rows[c];
                                    metadataValue =
                                        "    Col " +
                                        c + ": " +
                                        dr["ColumnName"];

                                    // column data
                                    if (rdr.IsDBNull(c) == true)
                                        dataValue = " is NULL";
                                    else
                                        dataValue =
                                            ": " +
                                            rdr.GetValue(c);

                                    // display column meta data and data
                                    output = metadataValue + dataValue;
                                    System.Diagnostics.Debug.WriteLine(output);

                                }
                                nRows++;
                            }
                            System.Diagnostics.Debug.WriteLine("   Total Rows: " +
                                nRows);
                        }
                    } while (rdr.NextResult());
                    Console.WriteLine("Total Result sets: " + results);

                    rdr.Close();
                    connPG.Close();
                }
                getMessage = "Успешно!";
                return true;

            }
            catch (Exception ex)
            {
                getMessage = "Неудачно!" + ex;
                return false;
            }
        }
    }
}