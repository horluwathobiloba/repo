using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Xml;
using Microsoft.AspNetCore.Hosting;

namespace ReventInject.Utilities
{
    public class BulkCopy
    {
        private readonly IWebHostEnvironment env;

        private string constr;
        public BulkCopy(IWebHostEnvironment env)
        {
            this.env = env;
        }

        public BulkCopy()
        {
            constr = null;

        }

        public BulkCopy(string con_str)
        {
            constr = con_str;
        }

        //1) StreamReader.ReadLine, Insert Line By Line
        /// <summary>
        /// Method1s the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="TableName">Name of the table.</param>
        /// <param name="isRowOneHeader">if set to <c>true</c> [is row one header].</param>
        /// <param name="isSavedInWebPath">if set to <c>true</c> [is saved in web path].</param>
        /// <param name="UploadID">The upload ID.</param>
        /// <param name="UploadIDColumnName">Name of the upload ID column.</param>
        /// <param name="ConnectionStr">The connection STR.</param>
        /// <param name="ColumnNames">The column names.</param><returns></returns>
        public bool Insert(string filename, string TableName, bool isRowOneHeader, bool isSavedInWebPath, long UploadID, string UploadIDColumnName, string ConnectionStr, params string[] ColumnNames)
        {
            var result = false;

            try
            {
                long i = 0;

                StreamReader sr = null;
                if (isSavedInWebPath)
                {
                    //var path = HttpContext.Current.Server.MapPath(filename);
                    var path = Path.Combine(env.WebRootPath, filename);
                    sr = new StreamReader(path);
                }
                else
                {
                    sr = new StreamReader(filename);
                }

                //if no data in file ‘manually’ throw an exception
                if (sr == null)
                {
                    throw new Exception("File Appears to be Empty");
                }


                SqlConnection dbConn = new SqlConnection(ConnectionStr);
                SqlCommand dbCmd = new SqlCommand();
                dbCmd.Connection = dbConn;

                //Dim wholeFile As String = sr.ReadToEnd()

                var qry1 = new StringBuilder();
                //start contruction of the insert part of the query
                qry1.AppendFormat("INSERT INTO {0} ", TableName);
                int icount = 0;
                qry1.Append("(");

                foreach (string x in ColumnNames)
                {
                    if (icount == ColumnNames.Length - 1)
                    {
                        qry1.AppendFormat("{0} ", x.ToUpper());
                    }
                    else
                    {
                        qry1.AppendFormat("{0}, ", x.ToUpper());
                    }
                    icount = icount + 1;
                }

                qry1.Append(") ");

                string line = sr.ReadLine();

                // Loop over each line in file, While list is Not Nothing.

                while ((line != null))
                {
                    if (isRowOneHeader & i <= 0)
                    {
                        //skip the header row
                    }
                    else
                    {
                        if (UploadID > 0)
                        {
                            line = line + "," + UploadID;
                        }

                        string[] fields = line.Split(',');
                        icount = 0;
                        var qry2 = new StringBuilder();

                        //start contruction of the value part of the query
                        qry2.AppendFormat(" VALUES (");


                        for (icount = 0; icount <= ColumnNames.Length - 1; icount++)
                        {
                            //get the type of the field
                            //Dim t = fields(icount).GetType()
                            //Dim tc As TypeCode = Type.GetTypeCode(t)
                            var x = ColumnNames[icount];
                            if (string.IsNullOrEmpty(fields[icount]))
                            {
                                qry2.AppendFormat("{0} ", null);
                            }
                            else
                            {
                                try
                                {
                                    // IsNumericType(t) Then
                                    if (Xtenxion.IsNumeric(fields[icount]))
                                    {
                                        if (icount == ColumnNames.Length - 1)
                                        {
                                            qry2.AppendFormat("{0} ", x.ToLower() == UploadIDColumnName.ToLower() ? (object)UploadID : string.IsNullOrEmpty(fields[icount]) ? (object)0 : (object)fields[icount]);
                                        }
                                        else
                                        {
                                            qry2.AppendFormat("{0}, ", x.ToLower() == UploadIDColumnName.ToLower() ? (object)UploadID : string.IsNullOrEmpty(fields[icount]) ? (object)0 : (object)fields[icount]);
                                        }
                                    }
                                    continue;

                                }
                                catch (Exception ex)
                                {
                                }

                                //tc = TypeCode.String Or tc = TypeCode.Char Or tc = TypeCode.Empty Or tc = TypeCode.Boolean Then
                                //type is not numeric
                                if (icount == ColumnNames.Length - 1)
                                {
                                    qry2.AppendFormat("'{0}' ", x.ToLower() == UploadIDColumnName.ToLower() ? (object)UploadID : string.IsNullOrEmpty(fields[icount]) ? null : fields[icount]);
                                }
                                else
                                {
                                    qry2.AppendFormat("'{0}', ", x.ToLower() == UploadIDColumnName.ToLower() ? (object)UploadID : string.IsNullOrEmpty(fields[icount]) ? null : fields[icount]);
                                }

                            }


                        }
                        qry2.AppendFormat(")");


                        var queryStr = qry1.ToString() + qry2.ToString();
                        dbCmd.CommandText = queryStr;

                        dbConn.Open();
                        dbCmd.ExecuteNonQuery();
                        dbConn.Close();
                    }

                    i = i + 1;
                    line = sr.ReadLine();
                }

                //Do


                //Loop While Not line = String.Empty

                result = true;

            }
            catch (Exception ex)
            {
            }

            return result;

        }

        /// <summary>
        /// Method12s the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="TableName">Name of the table.</param>
        /// <param name="isRowOneHeader">if set to <c>true</c> [is row one header].</param>
        /// <param name="isSavedInWebPath">if set to <c>true</c> [is saved in web path].</param>
        /// <param name="UploadReferenceID">The upload reference ID.</param>
        /// <param name="UploadReferenceIDColumnName">Name of the upload reference ID column.</param>
        /// <param name="ConnectionStr">The connection STR.</param>
        /// <param name="ColumnNames">The column names.</param><returns></returns>
        public bool Insert2(string filename, string TableName, bool isRowOneHeader, bool isSavedInWebPath, string UploadReferenceID, string UploadReferenceIDColumnName, string ConnectionStr, params string[] ColumnNames)
        {
            var result = false;

            try
            {
                long i = 0;

                StreamReader sr = null;
                if (isSavedInWebPath)
                {
                    var path = Path.Combine(env.WebRootPath, filename);
                    sr = new StreamReader(path);
                }
                else
                {
                    sr = new StreamReader(filename);
                }

                //if no data in file ‘manually’ throw an exception
                if (sr == null)
                {
                    throw new Exception("File Appears to be Empty");
                }


                SqlConnection dbConn = new SqlConnection(ConnectionStr);
                SqlCommand dbCmd = new SqlCommand();
                dbCmd.Connection = dbConn;

                //Dim wholeFile As String = sr.ReadToEnd()

                var qry1 = new StringBuilder();
                //start contruction of the insert part of the query
                qry1.AppendFormat("INSERT INTO {0} ", TableName);
                int icount = 0;
                qry1.Append("(");

                foreach (string x in ColumnNames)
                {
                    if (icount == ColumnNames.Length - 1)
                    {
                        qry1.AppendFormat("{0} ", x.ToUpper());
                    }
                    else
                    {
                        qry1.AppendFormat("{0}, ", x.ToUpper());
                    }
                    icount = icount + 1;
                }

                qry1.Append(") ");

                string line = sr.ReadLine();

                // Loop over each line in file, While list is Not Nothing.

                while ((line != null))
                {
                    if (isRowOneHeader & i <= 0)
                    {
                        //skip the header row
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(UploadReferenceID))
                        {
                            line = line + "," + "'" + UploadReferenceID + "'";
                        }

                        string[] fields = line.Split(',');
                        icount = 0;
                        var qry2 = new StringBuilder();

                        //start contruction of the value part of the query
                        qry2.AppendFormat(" VALUES (");


                        for (icount = 0; icount <= ColumnNames.Length - 1; icount++)
                        {
                            //get the type of the field
                            //Dim t = fields(icount).GetType()
                            //Dim tc As TypeCode = Type.GetTypeCode(t)
                            var x = ColumnNames[icount];
                            if (string.IsNullOrEmpty(fields[icount]))
                            {
                                qry2.AppendFormat("{0} ", null);
                            }
                            else
                            {
                                try
                                {
                                    // IsNumericType(t) Then
                                    if (Xtenxion.IsNumeric(fields[icount]))
                                    {
                                        if (icount == ColumnNames.Length - 1)
                                        {
                                            qry2.AppendFormat("{0} ", x.ToLower() == UploadReferenceIDColumnName.ToLower() ? UploadReferenceID : string.IsNullOrEmpty(fields[icount]) ? "0" : fields[icount]);
                                        }
                                        else
                                        {
                                            qry2.AppendFormat("{0}, ", x.ToLower() == UploadReferenceIDColumnName.ToLower() ? UploadReferenceID : string.IsNullOrEmpty(fields[icount]) ? "0" : fields[icount]);
                                        }
                                    }
                                    continue;

                                }
                                catch (Exception ex)
                                {
                                }

                                //tc = TypeCode.String Or tc = TypeCode.Char Or tc = TypeCode.Empty Or tc = TypeCode.Boolean Then
                                //type is not numeric
                                if (icount == ColumnNames.Length - 1)
                                {
                                    qry2.AppendFormat("'{0}' ", x.ToLower() == UploadReferenceIDColumnName.ToLower() ? UploadReferenceID : string.IsNullOrEmpty(fields[icount]) ? null : fields[icount]);
                                }
                                else
                                {
                                    qry2.AppendFormat("'{0}', ", x.ToLower() == UploadReferenceIDColumnName.ToLower() ? UploadReferenceID : string.IsNullOrEmpty(fields[icount]) ? null : fields[icount]);
                                }

                            }


                        }
                        qry2.AppendFormat(")");


                        var queryStr = qry1.ToString() + qry2.ToString();
                        dbCmd.CommandText = queryStr;

                        dbConn.Open();
                        dbCmd.ExecuteNonQuery();
                        dbConn.Close();
                    }

                    i = i + 1;
                    line = sr.ReadLine();
                }

                //Do


                //Loop While Not line = String.Empty

                result = true;

            }
            catch (Exception ex)
            {
            }

            return result;

        }

        //2) StreamReader.ReadLine, Batch Insert With DataAdapter
        /// <summary>
        /// Method2s the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="TableName">Name of the table.</param>
        /// <param name="isRowOneHeader">if set to <c>true</c> [is row one header].</param>
        /// <param name="isSavedInWebPath">if set to <c>true</c> [is saved in web path].</param>
        /// <param name="UploadID">The upload ID.</param>
        /// <param name="UploadIDColumnName">Name of the upload ID column.</param>
        /// <param name="ConnectionStr">The connection STR.</param>
        /// <param name="ColumnNames">The column names.</param><returns></returns>
        public bool Insert3(string filename, string TableName, bool isRowOneHeader, bool isSavedInWebPath, long UploadID, string UploadIDColumnName, string ConnectionStr, params string[] ColumnNames)
        {
            var result = false;

            try
            {
                long i = 0;
                SqlConnection dbConn = new SqlConnection(ConnectionStr);

                StreamReader sr = null;

                if (isSavedInWebPath)
                {
                    var path = Path.Combine(env.WebRootPath, filename);
                    sr = new StreamReader(path);
                }
                else
                {
                    sr = new StreamReader(filename);
                }

                //if no data in file ‘manually’ throw an exception
                if (sr == null)
                {
                    throw new Exception("File Appears to be Empty");
                }

                string line = sr.ReadLine();

                string[] strArray = line.Split(',');
                DataTable dt = new DataTable();
                DataRow row = null;

                var icount = 0;

                //use this block to generate a query that will be used to initialise the datatable and;
                // create the columns for the datatable
                var qry = new StringBuilder();
                qry.AppendFormat("SELECT TOP 1 ");

                foreach (string s in strArray)
                {
                    dt.Columns.Add(new DataColumn(ColumnNames[icount]));

                    if (icount == ColumnNames.Length - 1)
                    {
                        qry.AppendFormat("{0} ", ColumnNames[icount]);
                    }
                    else
                    {
                        qry.AppendFormat("{0}, ", ColumnNames[icount]);
                    }

                    icount = icount + 1;
                }
                qry.AppendFormat("{0}", TableName);
                var queryStr = qry.ToString();


                do
                {
                    row = dt.NewRow();
                    row.ItemArray = line.Split(',');

                    dt.Rows.Add(row);

                    i = i + 1;
                    line = sr.ReadLine();

                } while (!(line == string.Empty));

                SqlDataAdapter dataAdapter = new SqlDataAdapter();

                dataAdapter.SelectCommand = new SqlCommand(queryStr, dbConn);

                SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(dataAdapter);

                dbConn.Open();

                DataColumn col = new DataColumn();
                col.ColumnName = UploadIDColumnName;
                col.DataType = System.Type.GetType("System.Int64");
                col.DefaultValue = UploadID;
                dt.Columns.Add(col);
                //For icount = 0 To dt.Rows.Count - 1
                //    dt(icount)(UploadIDColumnName) = UploadID
                //Next

                dt.AcceptChanges();

                DataSet ds = new DataSet();
                dataAdapter.Fill(dt);

                dataAdapter.UpdateBatchSize = 1000;
                dataAdapter.Update(dt);

                dbConn.Close();
                result = true;

            }
            catch (Exception ex)
            {
            }

            return result;

        }

        /// <summary>
        /// Method22s the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="TableName">Name of the table.</param>
        /// <param name="isRowOneHeader">if set to <c>true</c> [is row one header].</param>
        /// <param name="isSavedInWebPath">if set to <c>true</c> [is saved in web path].</param>
        /// <param name="UploadReferenceID">The upload reference ID.</param>
        /// <param name="UploadReferenceIDColumnName">Name of the upload reference ID column.</param>
        /// <param name="ConnectionStr">The connection STR.</param>
        /// <param name="ColumnNames">The column names.</param><returns></returns>
        public bool BulkInsert(string filename, string TableName, bool isRowOneHeader, bool isSavedInWebPath, string UploadReferenceID, string UploadReferenceIDColumnName, string ConnectionStr, params string[] ColumnNames)
        {
            var result = false;

            try
            {
                long i = 0;
                SqlConnection dbConn = new SqlConnection(ConnectionStr);

                StreamReader sr = null;

                if (isSavedInWebPath)
                {
                    var path = Path.Combine(env.WebRootPath, filename);
                    //sr = new StreamReader(HttpContext.Current.Server.MapPath(filename));
                    sr = new StreamReader(path);
                }
                else
                {
                    sr = new StreamReader(filename);
                }

                //if no data in file ‘manually’ throw an exception
                if (sr == null)
                {
                    throw new Exception("File Appears to be Empty");
                }

                string line = sr.ReadLine();

                string[] strArray = line.Split(',');
                DataTable dt = new DataTable();
                DataRow row = null;

                var icount = 0;

                //use this block to generate a query that will be used to initialise the datatable and;
                // create the columns for the datatable
                var qry = new StringBuilder();
                qry.AppendFormat("SELECT TOP 1 ");

                foreach (string s in strArray)
                {
                    dt.Columns.Add(new DataColumn(ColumnNames[icount]));

                    if (icount == ColumnNames.Length - 1)
                    {
                        qry.AppendFormat("{0} ", ColumnNames[icount]);
                    }
                    else
                    {
                        qry.AppendFormat("{0}, ", ColumnNames[icount]);
                    }

                    icount = icount + 1;
                }
                qry.AppendFormat("{0}", TableName);
                var queryStr = qry.ToString();


                do
                {
                    row = dt.NewRow();
                    row.ItemArray = line.Split(',');

                    dt.Rows.Add(row);

                    i = i + 1;
                    line = sr.ReadLine();

                } while (!(line == string.Empty));

                SqlDataAdapter dataAdapter = new SqlDataAdapter();

                dataAdapter.SelectCommand = new SqlCommand(queryStr, dbConn);

                SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(dataAdapter);

                dbConn.Open();

                DataColumn col = new DataColumn();
                col.ColumnName = UploadReferenceIDColumnName;
                col.DataType = System.Type.GetType("System.String");
                col.DefaultValue = UploadReferenceID;
                dt.Columns.Add(col);
                //For icount = 0 To dt.Rows.Count - 1
                //    dt(icount)(UploadIDColumnName) = UploadID
                //Next

                dt.AcceptChanges();

                DataSet ds = new DataSet();
                dataAdapter.Fill(dt);

                dataAdapter.UpdateBatchSize = 1000;
                dataAdapter.Update(dt);

                dbConn.Close();
                result = true;

            }
            catch (Exception ex)
            {
            }

            return result;

        }

        // 3) StreamReader.ReadLine, SqlBulkCopy
        /// <summary>
        /// Method3s the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="TableName">Name of the table.</param>
        /// <param name="isRowOneHeader">if set to <c>true</c> [is row one header].</param>
        /// <param name="isSavedInWebPath">if set to <c>true</c> [is saved in web path].</param>
        /// <param name="ConnectionStr">The connection STR.</param>
        /// <param name="UploadID">The upload ID.</param>
        /// <param name="UploadIDColumnName">Name of the upload ID column.</param>
        /// <param name="ColumnNames">The column names.</param><returns></returns>
        public bool BulkCopyInsert(string filename, string TableName, bool isRowOneHeader, bool isSavedInWebPath, string ConnectionStr, long UploadID, string UploadIDColumnName, params string[] ColumnNames)
        {

            var result = false;

            try
            {
                long i = 0;
                SqlConnection dbConn = new SqlConnection(ConnectionStr);

                StreamReader sr = null;

                if (isSavedInWebPath)
                {
                    var path = Path.Combine(env.WebRootPath, filename);
                    //sr = new StreamReader(HttpContext.Current.Server.MapPath(filename));
                    sr = new StreamReader(path);
                }
                else
                {
                    sr = new StreamReader(filename);
                }

                //if no data in file ‘manually’ throw an exception
                if (sr == null)
                {
                    throw new Exception("File Appears to be Empty");
                }

                string line = sr.ReadLine();

                string[] strArray = line.Split(',');
                DataTable dt = new DataTable();
                DataRow row = null;

                var icount = 0;

                //use this block to generate a query that will be used to initialise the datatable and;
                // create the columns for the datatable
                var qry = new StringBuilder();
                qry.AppendFormat("SELECT TOP 1 ");

                foreach (string s in strArray)
                {
                    dt.Columns.Add(new DataColumn(ColumnNames[icount]));

                    if (icount == ColumnNames.Length - 1)
                    {
                        qry.AppendFormat("{0} ", ColumnNames[icount]);
                    }
                    else
                    {
                        qry.AppendFormat("{0}, ", ColumnNames[icount]);
                    }

                    icount = icount + 1;
                }
                qry.AppendFormat("{0}", TableName);
                var queryStr = qry.ToString();

                do
                {
                    if (i == 0 & isRowOneHeader)
                    {
                    }
                    else
                    {
                        row = dt.NewRow();
                        row.ItemArray = line.Split(',');
                        dt.Rows.Add(row);
                    }

                    i = i + 1;
                    line = sr.ReadLine();

                } while (!(line == string.Empty));

                DataColumn col = new DataColumn();
                col.ColumnName = UploadIDColumnName;
                col.DataType = System.Type.GetType("System.Int64");
                col.DefaultValue = UploadID;
                dt.Columns.Add(col);
                //For icount = 0 To dt.Rows.Count - 1
                //    dt(icount)(UploadIDColumnName) = UploadID
                //Next

                dt.AcceptChanges();

                SqlBulkCopy bc = new SqlBulkCopy(dbConn, SqlBulkCopyOptions.TableLock, null);
                bc.DestinationTableName = TableName;

                bc.BatchSize = dt.Rows.Count;

                dbConn.Open();
                bc.WriteToServer(dt);
                dbConn.Close();
                bc.Close();

                result = true;


            }
            catch (Exception ex)
            {
            }

            return result;

        }

        /// <summary>
        /// Method32s the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="TableName">Name of the table.</param>
        /// <param name="isRowOneHeader">if set to <c>true</c> [is row one header].</param>
        /// <param name="isSavedInWebPath">if set to <c>true</c> [is saved in web path].</param>
        /// <param name="ConnectionStr">The connection STR.</param>
        /// <param name="UploadReferenceID">The upload reference ID.</param>
        /// <param name="UploadReferenceIDColumnName">Name of the upload reference ID column.</param>
        /// <param name="ColumnNames">The column names.</param><returns></returns>
        public bool BulkCopy_With_FK(string filename, string TableName, bool isRowOneHeader, bool isSavedInWebPath, string ConnectionStr, string UploadReferenceID, string UploadReferenceIDColumnName, params string[] ColumnNames)
        {

            var result = false;

            try
            {
                long i = 0;
                SqlConnection dbConn = new SqlConnection(ConnectionStr);

                StreamReader sr = null;

                if (isSavedInWebPath)
                {
                    var path = Path.Combine(env.WebRootPath, filename);
                    sr = new StreamReader(path);
                }
                else
                {
                    sr = new StreamReader(filename);
                }

                //if no data in file ‘manually’ throw an exception
                if (sr == null)
                {
                    throw new Exception("File Appears to be Empty");
                }

                string line = sr.ReadLine();

                string[] strArray = line.Split(',');
                DataTable dt = new DataTable();
                DataRow row = null;

                var icount = 0;

                //use this block to generate a query that will be used to initialise the datatable and;
                // create the columns for the datatable
                var qry = new StringBuilder();
                qry.AppendFormat("SELECT TOP 1 ");

                foreach (string s in strArray)
                {
                    dt.Columns.Add(new DataColumn(ColumnNames[icount]));

                    if (icount == ColumnNames.Length - 1)
                    {
                        qry.AppendFormat("{0} ", ColumnNames[icount]);
                    }
                    else
                    {
                        qry.AppendFormat("{0}, ", ColumnNames[icount]);
                    }

                    icount = icount + 1;
                }
                qry.AppendFormat("{0}", TableName);
                var queryStr = qry.ToString();

                do
                {
                    if (i == 0 & isRowOneHeader)
                    {
                    }
                    else
                    {
                        row = dt.NewRow();
                        row.ItemArray = line.Split(',');
                        dt.Rows.Add(row);
                    }

                    i = i + 1;
                    line = sr.ReadLine();

                } while (!(line == string.Empty));

                DataColumn col = new DataColumn();
                col.ColumnName = UploadReferenceIDColumnName;
                col.DataType = System.Type.GetType("System.String");
                col.DefaultValue = UploadReferenceID;

                dt.Columns.Add(col);
                //For icount = 0 To dt.Rows.Count - 1
                //    dt(icount)(UploadIDColumnName) = UploadID
                //Next

                dt.AcceptChanges();

                SqlBulkCopy bc = new SqlBulkCopy(dbConn, SqlBulkCopyOptions.TableLock, null);
                bc.DestinationTableName = TableName;

                bc.BatchSize = dt.Rows.Count;

                dbConn.Open();
                bc.WriteToServer(dt);
                dbConn.Close();
                bc.Close();

                result = true;


            }
            catch (Exception ex)
            {
            }

            return result;

        }

        /// <summary>
        /// Method4s the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="TableName">Name of the table.</param>
        /// <param name="isRowOneHeader">if set to <c>true</c> [is row one header].</param>
        /// <param name="isSavedInWebPath">if set to <c>true</c> [is saved in web path].</param>
        /// <param name="ConnectionStr">The connection STR.</param>
        /// <param name="UploadID">The upload ID.</param>
        /// <param name="UploadIDColumnName">Name of the upload ID column.</param><returns></returns>
        public bool BulkInsertCSV(string filename, string TableName, bool isRowOneHeader, bool isSavedInWebPath, string ConnectionStr, long UploadID, string UploadIDColumnName)
        {

            var result = false;

            try
            {
                long i = 0;
                SqlConnection dbConn = new SqlConnection(ConnectionStr);

                var csvHandler = new CSVHandler();
                DataTable dt = CSVHandler.CSVToDataTable(filename, isRowOneHeader, isSavedInWebPath);

                var icount = 0;
                DataColumn idcol = new DataColumn();
                idcol.ColumnName = "ID";
                idcol.DataType = System.Type.GetType("System.Int64");
                idcol.AutoIncrement = true;
                idcol.AutoIncrementSeed = 1;
                idcol.AutoIncrementStep = 1;
                dt.Columns.Add(idcol);
                dt.AcceptChanges();
                dt.Columns[idcol.ColumnName].SetOrdinal(0);
                dt.AcceptChanges();

                DataColumn col = new DataColumn();
                col.ColumnName = UploadIDColumnName;
                col.DataType = System.Type.GetType("System.Int64");
                col.DefaultValue = UploadID;
                dt.Columns.Add(col);
                dt.AcceptChanges();

                for (icount = 0; icount <= dt.Rows.Count - 1; icount++)
                {
                    dt.Rows[icount][UploadIDColumnName] = UploadID;
                }

                dt.AcceptChanges();
                SqlBulkCopy bc = new SqlBulkCopy(dbConn, SqlBulkCopyOptions.TableLock, null);
                bc.DestinationTableName = TableName;

                bc.BatchSize = dt.Rows.Count;

                dbConn.Open();
                bc.WriteToServer(dt);
                dbConn.Close();
                bc.Close();

                result = true;


            }
            catch (Exception ex)
            {
            }

            return result;

        }

        /// <summary>
        /// Method42s the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="TableName">Name of the table.</param>
        /// <param name="isRowOneHeader">if set to <c>true</c> [is row one header].</param>
        /// <param name="isSavedInWebPath">if set to <c>true</c> [is saved in web path].</param>
        /// <param name="ConnectionStr">The connection STR.</param>
        /// <param name="UploadReferenceID">The upload reference ID.</param>
        /// <param name="UploadReferenceIDColumnName">Name of the upload reference ID column.</param><returns></returns>
        public bool BulkInsertCSV_With_FK(string filename, string TableName, bool isRowOneHeader, bool isSavedInWebPath, string ConnectionStr, string UploadReferenceID, string UploadReferenceIDColumnName)
        {

            var result = false;

            try
            {
                long i = 0;
                SqlConnection dbConn = new SqlConnection(ConnectionStr);

                DataTable dt = CSVHandler.CSVToDataTable(filename, isRowOneHeader, isSavedInWebPath);

                var icount = 0;
                DataColumn idcol = new DataColumn();
                idcol.ColumnName = "ID";
                idcol.DataType = System.Type.GetType("System.Int64");
                idcol.AutoIncrement = true;
                idcol.AutoIncrementSeed = 1;
                idcol.AutoIncrementStep = 1;
                dt.Columns.Add(idcol);
                dt.AcceptChanges();
                dt.Columns[idcol.ColumnName].SetOrdinal(0);
                dt.AcceptChanges();

                DataColumn col = new DataColumn();
                col.ColumnName = UploadReferenceIDColumnName;
                col.DataType = System.Type.GetType("System.String");
                col.DefaultValue = UploadReferenceID;
                dt.Columns.Add(col);
                dt.AcceptChanges();

                for (icount = 0; icount <= dt.Rows.Count - 1; icount++)
                {
                    dt.Rows[icount][UploadReferenceIDColumnName] = UploadReferenceID;
                }

                dt.AcceptChanges();
                SqlBulkCopy bc = new SqlBulkCopy(dbConn, SqlBulkCopyOptions.TableLock, null);
                bc.DestinationTableName = TableName;

                bc.BatchSize = dt.Rows.Count;

                dbConn.Open();
                bc.WriteToServer(dt);
                dbConn.Close();
                bc.Close();

                result = true;


            }
            catch (Exception ex)
            {
            }

            return result;

        }

        /// <summary>
        /// Method5s the specified dt.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <param name="TableName">Name of the table.</param>
        /// <param name="isRowOneHeader">if set to <c>true</c> [is row one header].</param>
        /// <param name="isSavedInWebPath">if set to <c>true</c> [is saved in web path].</param>
        /// <param name="ConnectionStr">The connection STR.</param>
        /// <param name="UploadID">The upload ID.</param>
        /// <param name="UploadIDColumnName">Name of the upload ID column.</param><returns></returns>
        public bool BulkInsertData(DataTable dt, string TableName, bool isRowOneHeader, bool isSavedInWebPath, string ConnectionStr, long UploadID, string UploadIDColumnName)
        {

            var result = false;

            try
            {
                long i = 0;
                SqlConnection dbConn = new SqlConnection(ConnectionStr);

                var icount = 0;

                DataColumn col = new DataColumn();
                col.ColumnName = UploadIDColumnName;
                col.DataType = System.Type.GetType("System.Int64");
                col.DefaultValue = UploadID;
                dt.Columns.Add(col);
                //For icount = 0 To dt.Rows.Count - 1
                //    dt(icount)(UploadIDColumnName) = UploadID
                //Next

                dt.AcceptChanges();
                SqlBulkCopy bc = new SqlBulkCopy(dbConn, SqlBulkCopyOptions.TableLock, null);
                bc.DestinationTableName = TableName;

                bc.BatchSize = dt.Rows.Count;

                dbConn.Open();
                bc.WriteToServer(dt);
                dbConn.Close();
                bc.Close();

                result = true;


            }
            catch (Exception ex)
            {
            }

            return result;

        }

        public bool BulkInsertData_With_FK(DataTable dt, string TableName, bool isRowOneHeader, bool isSavedInWebPath, string ConnectionStr, string UploadReferenceID, string UploadReferenceIDColumnName)
        {

            var result = false;

            try
            {
                SqlConnection dbConn = new SqlConnection(ConnectionStr);

                DataColumn col = new DataColumn();
                col.ColumnName = UploadReferenceIDColumnName;
                col.DataType = System.Type.GetType("System.Int64");
                col.DefaultValue = UploadReferenceID;
                dt.Columns.Add(col);
                //For icount = 0 To dt.Rows.Count - 1
                //    dt(icount)(UploadReferenceIDColumnName) = UploadReferenceID
                //Next

                dt.AcceptChanges();
                SqlBulkCopy bc = new SqlBulkCopy(dbConn, SqlBulkCopyOptions.TableLock, null);
                bc.DestinationTableName = TableName;

                bc.BatchSize = dt.Rows.Count;

                dbConn.Open();
                bc.WriteToServer(dt);
                dbConn.Close();
                bc.Close();

                result = true;


            }
            catch (Exception ex)
            {
            }

            return result;

        }

        public DataTable LoadCSVData(string filename, string TableName, bool isRowOneHeader, bool isSavedInWebPath)
        {

            var result = new DataTable();

            try
            {
                result = CSVHandler.CSVToDataTable(filename, isRowOneHeader, isSavedInWebPath);

            }
            catch (Exception ex)
            {
                Logger.WriteErrorToEventLog(ex);
            }

            return result;

        }

        /// <summary>
        /// Determines if a type is numeric.  Nullable numeric types are considered numeric.
        /// </summary>
        /// <remarks>
        /// Boolean is not considered numeric.
        /// </remarks>
        public static bool IsNumericType(Type type)
        {
            if (type == null)
            {
                return false;
            }

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
                case TypeCode.Object:
                    if (type.IsGenericType && object.ReferenceEquals(type.GetGenericTypeDefinition(), typeof(Nullable<>)))
                    {
                        return IsNumericType(Nullable.GetUnderlyingType(type));
                    }
                    return false;
            }
            return false;

        }

        /// <summary>
        /// Determines if a type is numeric.  Nullable numeric types are considered numeric.
        /// </summary>
        /// <remarks>
        /// Boolean is not considered numeric.
        /// </remarks>
        /// 
        public static TypeCode GetNumericType(Type type)
        {
            if (type == null)
            {
                return TypeCode.Empty;
            }

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return Type.GetTypeCode(type);
                case TypeCode.Object:
                    if (type.IsGenericType && object.ReferenceEquals(type.GetGenericTypeDefinition(), typeof(Nullable<>)))
                    {
                        return GetNumericType(Nullable.GetUnderlyingType(type));
                    }
                    return TypeCode.Empty;
            }
            return TypeCode.Empty;

        }

        public bool BulkCopyInsert(DataTable dt, string TableName, string ConnectionStr)
        {

            var result = false;

            try
            {
                SqlConnection dbConn = new SqlConnection(ConnectionStr);
                DataColumn idcol = new DataColumn();

                Console.WriteLine("Creating identity column...");

                idcol.ColumnName = "ID";
                idcol.DataType = System.Type.GetType("System.Int64");
                idcol.AutoIncrement = true;
                idcol.AutoIncrementSeed = 1;
                idcol.AutoIncrementStep = 1;
                dt.Columns.Add(idcol);
                dt.AcceptChanges();
                dt.Columns[idcol.ColumnName].SetOrdinal(0);
                dt.AcceptChanges();

                Console.WriteLine("Identity column created...");
                SqlBulkCopy bc = new SqlBulkCopy(dbConn, SqlBulkCopyOptions.TableLock, null);
                bc.DestinationTableName = TableName;

                bc.BatchSize = dt.Rows.Count;

                dbConn.Open();
                Console.WriteLine("bulk insert started...");
                bc.WriteToServer(dt);
                Console.WriteLine("bulk insert completed...");
                dbConn.Close();
                bc.Close();

                result = true;


            }
            catch (Exception ex)
            {
                Logger.WriteErrorToEventLog(ex);
            }

            return result;

        }

        public bool BulkCopyInsert(DataTable dt, string TableName, string ConnectionStr, string PKeyColumnName)
        {

            var result = false;

            try
            {
                using (SqlConnection dbConn = new SqlConnection(ConnectionStr))
                {

                    using (DataColumn idcol = new DataColumn())
                    {
                        idcol.ColumnName = PKeyColumnName;
                        idcol.DataType = System.Type.GetType("System.Int64");
                        idcol.AutoIncrement = true;
                        idcol.AutoIncrementSeed = 1;
                        idcol.AutoIncrementStep = 1;
                        dt.Columns.Add(idcol);
                        dt.AcceptChanges();
                        dt.Columns[idcol.ColumnName].SetOrdinal(0);
                        dt.AcceptChanges();

                        using (SqlBulkCopy bc = new SqlBulkCopy(dbConn, SqlBulkCopyOptions.TableLock, null))
                        {
                            bc.DestinationTableName = TableName;

                            bc.BatchSize = dt.Rows.Count;

                            dbConn.Open();
                            bc.WriteToServer(dt);
                            dbConn.Close();
                            bc.Close();

                            result = true;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
            }

            return result;

        }

        public bool BulkCopyInsert_WithForeignKey(DataTable dt, string TableName, string ConnectionStr, string FK_Value, string FK_ColumnName, ref string StrErr)
        {

            var result = false;

            try
            {

                using (SqlConnection dbConn = new SqlConnection(ConnectionStr))
                {

                    var icount = 0;
                    using (DataColumn idcol = new DataColumn())
                    {
                        idcol.ColumnName = "ID";
                        idcol.DataType = System.Type.GetType("System.Int64");
                        idcol.AutoIncrement = true;
                        idcol.AutoIncrementSeed = 1;
                        idcol.AutoIncrementStep = 1;

                        dt.Columns.Add(idcol);
                        dt.AcceptChanges();
                        dt.Columns[idcol.ColumnName].SetOrdinal(0);
                        dt.AcceptChanges();

                        using (DataColumn refcol = new DataColumn())
                        {
                            refcol.ColumnName = FK_ColumnName;
                            refcol.DataType = System.Type.GetType("System.String");

                            dt.Columns.Add(refcol);
                            dt.AcceptChanges();

                            for (icount = 0; icount <= dt.Rows.Count - 1; icount++)
                            {
                                dt.Rows[icount][FK_ColumnName] = FK_Value;
                            }

                            dt.AcceptChanges();

                            using (SqlBulkCopy bc = new SqlBulkCopy(dbConn, SqlBulkCopyOptions.TableLock, null))
                            {

                                bc.DestinationTableName = TableName;
                                bc.BatchSize = dt.Rows.Count;

                                foreach (DataColumn col in dt.Columns)
                                {
                                    bc.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                                }

                                dbConn.Open();
                                bc.WriteToServer(dt);
                                dbConn.Close();
                                bc.Close();

                                result = true;
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                StrErr = ex.Message;
            }

            return result;

        }

        public bool BulkCopyInsert_WithForeignKey(string xml, string TableName, string ConnectionStr, string FK_Value, string FK_ColumnName, ref string StrErr)
        {

            var result = false;
            var dt = new DataTable();

            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(xml);
                        writer.Flush();
                        stream.Position = 0;

                        var x = new XmlDocument();
                        x.Load(stream);

                        XmlNodeReader xmlNodeRdr = new XmlNodeReader(x);
                        using (DataSet ds = new DataSet())
                        {
                            var dd = new DataTable();
                            ds.ReadXml(xmlNodeRdr);
                            dt = ds.Tables[0].Copy();
                        }
                    }
                }

                long i = 0;
                using (SqlConnection dbConn = new SqlConnection(ConnectionStr))
                {

                    var icount = 0;
                    using (DataColumn idcol = new DataColumn())
                    {
                        idcol.ColumnName = "ID";
                        idcol.DataType = System.Type.GetType("System.Int64");
                        idcol.AutoIncrement = true;
                        idcol.AutoIncrementSeed = 1;
                        idcol.AutoIncrementStep = 1;

                        dt.Columns.Add(idcol);
                        dt.AcceptChanges();
                        dt.Columns[idcol.ColumnName].SetOrdinal(0);
                        dt.AcceptChanges();

                        using (DataColumn refcol = new DataColumn())
                        {
                            refcol.ColumnName = FK_ColumnName;
                            refcol.DataType = System.Type.GetType("System.String");

                            dt.Columns.Add(refcol);
                            dt.AcceptChanges();

                            for (icount = 0; icount <= dt.Rows.Count - 1; icount++)
                            {
                                dt.Rows[icount][FK_ColumnName] = FK_Value;
                            }

                            dt.AcceptChanges();

                            using (SqlBulkCopy bc = new SqlBulkCopy(dbConn, SqlBulkCopyOptions.TableLock, null))
                            {

                                bc.DestinationTableName = TableName;
                                bc.BatchSize = dt.Rows.Count;

                                foreach (DataColumn col in dt.Columns)
                                {
                                    bc.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                                }

                                dbConn.Open();
                                bc.WriteToServer(dt);
                                dbConn.Close();
                                bc.Close();

                                result = true;
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                StrErr = ex.Message;
            }

            return result;

        }

    }

}


