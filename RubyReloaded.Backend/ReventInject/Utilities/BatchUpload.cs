using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using System.Data.OleDb;
using System.Linq;
using System.Reflection;

namespace ReventInject.Utilities
{

    public class BatchUpload<T>
    {

        private string _Error;

        private T _ObjectType;
        public string ErrorDescription
        {
            get { return _Error; }
            set { _Error = value; }
        }

        public T Obj
        {
            get { return _ObjectType; }
            set { _ObjectType = value; }
        }



    }

    public class SQLBatchUpload
    {
        private static string constr;
        public SQLBatchUpload(string _constr)
        {
            constr = _constr;
        }

        public class BulkUpload<T>
        {



            public static DataTable ToDataTable(List<T> objList)
            {

                Type elementType = typeof(T);
                DataTable tb = new DataTable();

                //add a column to table for each public property on T 
                foreach (PropertyInfo propInfo in elementType.GetProperties())
                {
                    tb.Columns.Add(propInfo.Name, propInfo.PropertyType);
                }

                //go through each property on T and add each value to the table 
                foreach (T item in objList)
                {
                    DataRow row = tb.NewRow();
                    foreach (PropertyInfo propInfo in elementType.GetProperties())
                    {
                        row[propInfo.Name] = propInfo.GetValue(item, null);
                    }

                    //This line was missing: 
                    tb.Rows.Add(row);
                }


                return tb;

            }

            public static string BuildInsertQuery()
            {

                Type objType = typeof(T);
                string dbTableName = "";
                StringBuilder sb = new StringBuilder();

                try
                {
                    if (SQLBatchUpload.LastAlphas.Contains(typeof(T).Name.Last()))
                    {
                        dbTableName = typeof(T).Name + "es";
                    }
                    else
                    {
                        dbTableName = typeof(T).Name + "s";
                    }

                }
                catch (Exception ex)
                {
                }
                string query = string.Empty;

                try
                {
                    //start building the insert query
                    sb.Append("INSERT INTO " + dbTableName + "(");

                    //add a column names to query for each public property on T 
                    foreach (PropertyInfo propInfo in objType.GetProperties())
                    {
                        sb.Append(propInfo.Name + ", ");
                    }
                    // remove the last comma
                    sb = sb.Remove(sb.Length - 1, 1);
                    sb.Append(") VALUES(");

                    //add each parameter value
                    foreach (PropertyInfo propInfo in objType.GetProperties())
                    {
                        var param_name = "@" + propInfo.Name;
                        sb.Append(param_name + ", ");
                    }
                    sb = sb.Remove(sb.Length - 1, 1);
                    sb.Append(")");
                    query = sb.ToString();


                }
                catch (Exception ex)
                {
                }

                return query;

            }

            public static int InsertWithoutTransaction(List<T> objList)
            {

                var dt = ToDataTable(objList);
                var result = 0;


                try
                {
                    using (SqlConnection destinationConnection = new SqlConnection(constr))
                    {
                        destinationConnection.Open();

                        SqlCommand myCommand = new SqlCommand();
                        myCommand.CommandText = BuildInsertQuery();
                        myCommand.Connection = destinationConnection;

                        foreach (DataRow row in dt.Rows)
                        {
                            int col_index = 0;
                            //Add the parameters
                            myCommand.Parameters.Clear();

                            for (col_index = 0; col_index <= row.ItemArray.Length - 1; col_index++)
                            {
                                string col_param = "@" + row.Table.Columns[col_index].ColumnName;
                                myCommand.Parameters.AddWithValue(col_param, row[col_index]);
                                col_index += 1;
                            }

                            try
                            {
                                //Insert the record!
                                myCommand.ExecuteNonQuery();
                                result += 1;

                            }
                            catch (Exception ex)
                            {
                            }

                        }

                        destinationConnection.Close();
                    }


                }
                catch (Exception ex)
                {
                }

                return result;

            }

            public static int InsertOneAtATimeWithoutTransaction(List<T> objList)
            {
                Type elementType = typeof(T);
                StringBuilder sb = new StringBuilder();
                int result = 0;

                try
                {
                    string query = BuildInsertQuery();

                    using (SqlConnection destinationConnection = new SqlConnection(constr))
                    {
                        destinationConnection.Open();

                        SqlCommand myCommand = new SqlCommand();
                        myCommand.CommandText = query;
                        myCommand.Connection = destinationConnection;


                        foreach (T item in objList)
                        {
                            myCommand.Parameters.Clear();

                            foreach (PropertyInfo propInfo in elementType.GetProperties())
                            {
                                string param = "@" + propInfo.Name;
                                myCommand.Parameters.AddWithValue(param, propInfo.GetValue(item, null));
                            }
                            try
                            {
                                //Insert the record!
                                myCommand.ExecuteNonQuery();
                                result += 1;

                            }
                            catch (Exception ex)
                            {
                            }


                        }

                        destinationConnection.Close();
                    }


                }
                catch (Exception ex)
                {
                }

                return result;

            }

            public static int InsertOneAtATimeWithoutTransaction(DataTable dt)
            {
                int result = 0;


                try
                {
                    using (SqlConnection destinationConnection = new SqlConnection(constr))
                    {
                        destinationConnection.Open();

                        SqlCommand myCommand = new SqlCommand();
                        myCommand.CommandText = BuildInsertQuery();
                        myCommand.Connection = destinationConnection;

                        foreach (DataRow row in dt.Rows)
                        {
                            int col_index = 0;
                            //Add the parameters
                            myCommand.Parameters.Clear();

                            for (col_index = 0; col_index <= row.ItemArray.Length - 1; col_index++)
                            {
                                string col_param = "@" + row.Table.Columns[col_index].ColumnName;
                                myCommand.Parameters.AddWithValue(col_param, row[col_index]);
                                col_index += 1;
                            }
                            try
                            {
                                //Insert the record!
                                myCommand.ExecuteNonQuery();
                                result += 1;

                            }
                            catch (Exception ex)
                            {
                            }
                        }

                        destinationConnection.Close();
                    }


                }
                catch (Exception ex)
                {
                }

                return result;

            }

            public static bool InsertViaBulkCopyWithoutTransaction(DataTable dt, string conStr)
            {
                bool result = false;

                try
                {
                    string dbTableName = "";
                    try
                    {
                        if (SQLBatchUpload.LastAlphas.Contains(typeof(T).Name.Last()))
                        {
                            dbTableName = typeof(T).Name + "es";
                        }
                        else
                        {
                            dbTableName = typeof(T).Name + "s";
                        }

                    }
                    catch (Exception ex)
                    {
                    }

                    using (SqlConnection destinationConnection = new SqlConnection(conStr))
                    {
                        destinationConnection.Open();

                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(destinationConnection))
                        {
                            bulkCopy.DestinationTableName = dbTableName;

                            //You can optionally specify the batch size... by default, all records are sent to the database in one batch
                            bulkCopy.BatchSize = 100;

                            //Define column mappings
                            foreach (DataColumn col in dt.Columns)
                            {
                                bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                            }

                            bulkCopy.WriteToServer(dt);
                            result = true;
                        }

                        destinationConnection.Close();
                    }



                }
                catch (Exception ex)
                {
                }

                return result;

            }

            public static bool InsertViaSqlBulkCopyWithInternalTransaction(DataTable dt, string conStr)
            {

                bool result = false;
                string dbTableName = "";

                if (SQLBatchUpload.LastAlphas.Contains(typeof(T).Name.Last()))
                {
                    dbTableName = typeof(T).Name + "es";
                }
                else
                {
                    dbTableName = typeof(T).Name + "s";
                }

                try
                {
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conStr, SqlBulkCopyOptions.UseInternalTransaction))
                    {
                        bulkCopy.DestinationTableName = dbTableName;

                        //Define column mappings
                        foreach (DataColumn col in dt.Columns)
                        {
                            bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                        }

                        //When using an internal transaction, a new transaction is created for each BatchSize number of records.
                        //(If BatchSize = 0 (the default) then the entire bulk operation occurs under a single transaction)
                        bulkCopy.BatchSize = 100;

                        try
                        {
                            bulkCopy.WriteToServer(dt);
                            result = true;
                        }
                        catch (Exception ex)
                        {
                            //Utility.Notify = "There was an error!" & ex.Message
                            //Client_Alert("THERE WAS AN ERROR: " & ex.Message)
                        }
                    }


                }
                catch (Exception ex)
                {
                }

                return result;

            }

            public static bool UploadExcel(string filename, string WorkSheetName, string conStr)
            {
                bool retval = false;
                string rootpath = "C:\\BulkUploads\\";
                //Make sure the demo Excel file was uploaded
                if (Path.GetExtension(filename).ToLower() != ".xls")
                {
                    //  Response.Write("<h1>ERROR: Upload only works with Excel 97-2003 file format...</h1>")
                    throw new Exception("ERROR: Upload only works with Excel 97-2003 file format..."); 
                }
                try
                {
                    string filepath = rootpath + Path.GetFileName(filename);
                    File.Create(filepath);

                    string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + filename + ";" + "Extended Properties=Excel 8.0;";

                    DataTable excelData = new DataTable();

                    using (OleDbConnection myConnection = new OleDbConnection(connectionString))
                    {
                        //Get all data from the InventoryData worksheet
                        OleDbCommand myCommand = new OleDbCommand();
                        myCommand.CommandText = "SELECT * FROM [" + WorkSheetName + "$]";
                        myCommand.Connection = myConnection;

                        //Read data into DataTable
                        OleDbDataAdapter myAdapter = new OleDbDataAdapter();
                        myAdapter.SelectCommand = myCommand;
                        myAdapter.Fill(excelData);

                        myConnection.Close();
                    }

                    //InsertOneAtATimeWithoutTransaction(excelData)
                    //InsertViaSqlBulkCopyWithoutTransaction(excelData)
                    InsertViaSqlBulkCopyWithInternalTransaction(excelData, conStr);
                    retval= true;
                }
                catch (Exception ex)
                {
                    retval = false;
                }
                return retval;
                //Save the uploaded Excel spreadsheet to ~/Uploads
                //Finally, delete the uploaded file
                //File.Delete(filepath)

            }

        }


        public static void InsertViaSqlBulkCopyWithoutTransaction(DataTable dt, string dbTableName, string conStr)
        {
            using (SqlConnection destinationConnection = new SqlConnection(conStr))
            {
                destinationConnection.Open();

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(destinationConnection))
                {
                    bulkCopy.DestinationTableName = dbTableName;

                    //You can optionally specify the batch size... by default, all records are sent to the database in one batch
                    bulkCopy.BatchSize = 100;

                    //Define column mappings
                    foreach (DataColumn col in dt.Columns)
                    {
                        bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                    }


                    bulkCopy.WriteToServer(dt);
                }

                destinationConnection.Close();
            }

        }


        public static void InsertViaSqlBulkCopyWithInternalTransaction(DataTable dt, string dbTableName, string conStr)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conStr, SqlBulkCopyOptions.UseInternalTransaction))
            {
                bulkCopy.DestinationTableName = dbTableName;

                //Define column mappings
                foreach (DataColumn col in dt.Columns)
                {
                    bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                }

                //When using an internal transaction, a new transaction is created for each BatchSize number of records.
                //(If BatchSize = 0 (the default) then the entire bulk operation occurs under a single transaction)
                bulkCopy.BatchSize = 100;

                try
                {
                    bulkCopy.WriteToServer(dt);

                }
                catch (Exception ex)
                {
                    //Utility.Notify = "There was an error!" & ex.Message
                    //Client_Alert("THERE WAS AN ERROR: " & ex.Message)
                }
            }
        }

        private byte[] StreamFile2(string filename)
        {

            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite);
            // Create a byte array of file stream length 
            byte[] ImageData = new byte[fs.Length];
            //Read block of bytes from stream into the byte array
            fs.Read(ImageData, 0, System.Convert.ToInt32(fs.Length));
            //Close the File Stream  
            fs.Close();
            //return the byte data  
            return ImageData;

        }

        private byte[] StreamFile(string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            // Create a byte array of file stream length   
            byte[] ImageData = new byte[fs.Length];
            //Read block of bytes from stream into the byte array   
            fs.Read(ImageData, 0, System.Convert.ToInt32(fs.Length));
            //Close the File Stream   
            fs.Close();
            return ImageData;
            //return the byte data
        }

        private byte[] GetStreamAsByteArray(System.IO.Stream stream)
        {
            int streamLength = Convert.ToInt32(stream.Length);
            byte[] fileData = new byte[streamLength + 1];
            // Read the file into a byte array8.       
            stream.Read(fileData, 0, streamLength);
            stream.Close();

            return fileData;
        }

        private static string LastAlphas
        {
            get { return "schx"; }
        }

    }


}
