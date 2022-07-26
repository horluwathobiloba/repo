using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Text;
using System.Reflection;
using System.Linq;
using System.Threading.Tasks;
using ReventInject.Utilities;

namespace ReventInject.DataAccess
{
    public class ADOOleDbExtensions
    {
        private string conStr;
        private OleDbTransaction Txn;
        private bool InvokeTxn;
        private char[] remove_xters = new char[] { ':', '*', '?', '"', '<', '>', '|', '/', '\'', '^', '{', '}', ',' };

        public ADOOleDbExtensions(string _conStr)
        {
            conStr = _conStr;
        }

        public ADOOleDbExtensions(string _conStr, bool _InvokeTxn)
        {
            conStr = _conStr;
            InvokeTxn = _InvokeTxn;
        }

        public string ExecuteScalar(string StrQuery)
        {
            try
            {
                using (OleDbConnection oracleConn = new OleDbConnection(conStr))
                {
                    oracleConn.Open();
                    using (OleDbCommand ocomd = new OleDbCommand(StrQuery, oracleConn))
                    {
                        object obj = ocomd.ExecuteScalar();
                        if (obj is DBNull || obj == null)
                        {
                            return "";
                        }
                        else
                        {
                            return obj.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return "SQLError:" + ex.Message;
            }

        }

        public IList<T> LoadQueryData<T>(string query)
        {

            using (OleDbConnection conn = new OleDbConnection(conStr))
            {
                using (DataTable dt = new DataTable())
                {
                    var result = new List<T>();
                    try
                    {
                        using (OleDbCommand cmd = new OleDbCommand(query, conn))
                        {
                            conn.Open();
                            OleDbDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                            dt.Load(dr);

                            if (dt != null)
                            {
                                result = DALExtensions.CollectionHelper.ConvertTo<T>(dt).ToList();
                            }
                        }
                    }
                    catch (OleDbException ex)
                    {
                        // handle error
                        result = new List<T>();
                        Logger.WriteErrorToEventLog((Exception)ex);
                    }
                    catch (Exception ex)
                    {
                        // handle error
                        Logger.WriteErrorToEventLog(ex);
                    }
                    finally
                    {
                        conn.Close();
                    }

                    return result;
                }
            }
        }

        public DataTable LoadDataTable(string query)
        {

            using (OleDbConnection conn = new OleDbConnection(conStr))
            {
                using (DataTable dt = new DataTable())
                {
                    var result = new DataTable();
                    try
                    {
                        using (OleDbCommand cmd = new OleDbCommand(query, conn))
                        {
                            conn.Open();
                            OleDbDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                            dt.Load(dr);

                            if (dt != null)
                            {
                                result = dt;
                            }
                        }
                    }
                    catch (OleDbException ex)
                    {
                        // handle error
                        result = new DataTable();
                        Logger.WriteErrorToEventLog((Exception)ex);
                    }
                    catch (Exception ex)
                    {
                        // handle error
                        Logger.WriteErrorToEventLog(ex);
                    }
                    finally
                    {
                        conn.Close();
                    }

                    return result;
                }
            }
        }

        public bool RunQuery(string query)
        {

            using (OleDbConnection conn = new OleDbConnection(conStr))
            {
                var result = false;
                try
                {
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        conn.Open();
                        var i = cmd.ExecuteNonQuery();

                        if (i > 0)
                        {
                            result = true;
                        }
                    }
                }
                catch (OleDbException ex)
                {
                    // handle error
                    Logger.WriteErrorToEventLog((Exception)ex);
                    return false;
                }
                catch (Exception ex)
                {
                    // handle error
                    Logger.WriteErrorToEventLog(ex);
                }
                finally
                {
                    conn.Close();
                }

                return result;
            }
        }

        private OleDbDataReader ReadData(string qry)
        {
            OleDbConnection con = new OleDbConnection();
            OleDbCommand cmd = new OleDbCommand();
            try
            {
                con.ConnectionString = conStr;
                con.Open();

                cmd.Connection = con;
                cmd.CommandText = qry;
                OleDbDataReader odr = cmd.ExecuteReader();
                return odr;
            }
            catch (OleDbException ex)
            {
                Logger.WriteErrorToEventLog((Exception)ex);
                return null;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorToEventLog(ex);
                return null;
            }
            finally
            {
                con.Close();
                con.Dispose();
                cmd.Dispose();
            }

        }

        public bool RecordExists(string query)
        {
            OleDbConnection con = new OleDbConnection();
            OleDbCommand cmd = new OleDbCommand();

            try
            {
                con.ConnectionString = conStr;
                con.Open();

                cmd.Connection = con;
                cmd.CommandText = query;
                OleDbDataReader odr = cmd.ExecuteReader();

                return odr.HasRows;

            }
            catch (OleDbException ex)
            {
                Logger.WriteErrorToEventLog((Exception)ex);
                return false;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorToEventLog(ex);
                return false;
            }
            finally
            {
                con.Close();
                con.Dispose();
                cmd.Dispose();
            }

        }

        public bool ExecuteMany(List<string> queryStrings)
        {
            using (OleDbConnection connection = new OleDbConnection(conStr))
            {
                try
                {
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand())
                    {
                        command.Connection = connection;
                        if (InvokeTxn)
                        {
                            Txn = connection.BeginTransaction(IsolationLevel.ReadCommitted);
                            command.Transaction = Txn;
                        }

                        Parallel.ForEach(queryStrings, (x) =>
                             {
                                 command.CommandText = x;
                                 var count = command.ExecuteNonQuery();
                             });

                        if (InvokeTxn)
                        {
                            Txn.Commit();
                        }

                        return true;

                    }

                }
                catch (Exception ex)
                {
                    if (InvokeTxn)
                    {
                        Txn.Rollback();
                    }

                    Logger.WriteErrorToEventLog(ex);
                }
                return false;
            }
        }

        public bool ExecuteQuery(string queryString)
        {
            using (OleDbConnection connection = new OleDbConnection(conStr))
            {
                try
                {
                    connection.Open();
                    OleDbCommand command = new OleDbCommand(queryString, connection);

                    if (InvokeTxn)
                    {
                        Txn = connection.BeginTransaction(IsolationLevel.ReadCommitted);
                        command.Transaction = Txn;
                    }

                    var count = command.ExecuteNonQuery();

                    if (InvokeTxn)
                    {
                        Txn.Commit();
                    }

                    if (count > 0)
                    {
                        return true;
                    }
                }

                catch (Exception ex)
                {
                    if (InvokeTxn)
                    {
                        Txn.Rollback();
                    }

                    Logger.WriteErrorToEventLog(ex);

                }
                return false;
            }
        }

        private string BuildInsertQuery<T>(T obj, string TableName)
        {
            var list = new List<T>();
            list.Add(obj);

            var tb = DALExtensions.CollectionHelper.ConvertTo<T>(list);

            var qry1 = new StringBuilder();
            //start contruction of the insert part of the query
            qry1.AppendFormat("INSERT INTO {0} ", TableName);


            int icount = 0;
            qry1.Append("(");

            foreach (DataColumn x in tb.Columns)
            {
                if (icount == tb.Columns.Count - 1)
                {
                    qry1.AppendFormat("{0} ", x.ColumnName.ToUpper());
                }
                else
                {
                    qry1.AppendFormat("{0}, ", x.ColumnName.ToUpper());
                }
                icount = icount + 1;
            }

            qry1.Append(") ");

            Type elementType = typeof(T);
            var props = elementType.GetProperties();
            var cls = elementType.GetMembers().Where(x => x.MemberType == MemberTypes.Property | x.MemberType == MemberTypes.NestedType | x.MemberType == MemberTypes.Field).ToList();


            //loop over each item in the list 
            foreach (var item in list)
            {
                try
                {
                    icount = 0;
                    var qry2 = new StringBuilder();

                    //start contruction of the value part of the query
                    qry2.AppendFormat(" VALUES (");

                    foreach (DataColumn dc in tb.Columns)
                    {
                        var cnam = dc.ColumnName.ToLower();
                        var chk = (from p in cls where p.Name.Equals(cnam, StringComparison.OrdinalIgnoreCase) select p).ToList();

                        if (chk.Count > 0)
                        {
                            var c = props.Where(h => h.Name.Equals(cnam, StringComparison.OrdinalIgnoreCase)).First();
                            object val = null;
                            try
                            {
                                val = c.GetValue(item, null);
                            }
                            catch (Exception ex)
                            {
                            }


                            try
                            {
                                // IsNumericType(t) Then
                                if (val == null)
                                {
                                    if (icount == tb.Columns.Count - 1)
                                    {
                                        qry2.AppendFormat("{0} ", "NULL");
                                    }
                                    else
                                    {
                                        qry2.AppendFormat("{0}, ", "NULL");
                                    }

                                }
                                else if (c.PropertyType.Name.Equals("string", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (!val.IsDate())
                                    {
                                        val = ((string)val).RemoveSpecialCharacters(remove_xters);
                                    }

                                    if (icount == tb.Columns.Count - 1)
                                    {
                                        qry2.AppendFormat("{0} ", val == null ? "NULL" : string.Format("'{0}'", (val.IsDate() ? val.ToSQLDate() : val)));
                                    }
                                    else
                                    {
                                        qry2.AppendFormat("{0}, ", val == null ? "NULL" : string.Format("'{0}'", (val.IsDate() ? val.ToSQLDate() : val)));
                                    }

                                }
                                else if (Xtenxion.IsNumeric(val) || c.PropertyType.FullName.ToLower().Contains("decimal")
                                || c.PropertyType.FullName.ToLower().Contains("int")
                                || c.PropertyType.FullName.ToLower().Contains("long")
                                || c.PropertyType.FullName.ToLower().Contains("double")
                                || c.PropertyType.FullName.ToLower().Contains("float"))
                                {
                                    if (icount == tb.Columns.Count - 1)
                                    {
                                        qry2.AppendFormat("{0} ", val == null ? 0 : val);
                                    }
                                    else
                                    {
                                        qry2.AppendFormat("{0}, ", val == null ? 0 : val);
                                    }
                                }
                                else
                                {
                                    //tc = TypeCode.String Or tc = TypeCode.Char Or tc = TypeCode.Empty Or tc = TypeCode.Boolean Then
                                    //type is not numeric
                                    if (!val.IsDate() && !c.PropertyType.FullName.ToLower().Contains("date"))
                                    {
                                        val = ((string)val).RemoveSpecialCharacters(remove_xters);
                                    }

                                    if (icount == tb.Columns.Count - 1)
                                    {
                                        qry2.AppendFormat("{0} ", val == null ? "NULL" : string.Format("'{0}'", (val.IsDate() ? val.ToSQLDate() : val)));
                                    }
                                    else
                                    {
                                        qry2.AppendFormat("{0}, ", val == null ? "NULL" : string.Format("'{0}'", (val.IsDate() ? val.ToSQLDate() : val)));
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                if (icount == tb.Columns.Count - 1)
                                {
                                    qry2.AppendFormat("{0} ", "NULL");
                                }
                                else
                                {
                                    qry2.AppendFormat("{0}, ", "NULL");
                                }
                            }

                        }
                        icount = icount + 1;
                    }

                    qry2.AppendFormat(")");


                    var strQuery = qry1.ToString() + qry2.ToString();

                    if (!string.IsNullOrEmpty(strQuery))
                    {
                        return strQuery;
                    }

                }
                catch (Exception ex)
                {
                    Logger.WriteErrorToEventLog(ex);
                }
            }

            return null;
        }

        public bool Insert<T>(T obj, string TableName)
        {
            var qry = BuildInsertQuery(obj, TableName);
            var result = ExecuteQuery(qry);
            return result;
        }

        public string BuildUpdateQuery<T>(T obj, string TableName, string IDColumnName, string ID)
        {

            var list = new List<T>();
            list.Add(obj);

            var tb = DALExtensions.CollectionHelper.ConvertTo<T>(list);

            var qry2 = new StringBuilder();
            //start contruction of the insert part of the query
            qry2.AppendFormat("UPDATE {0} SET ", TableName);

            int icount = 0;

            Type elementType = typeof(T);
            var props = elementType.GetProperties();
            var cls = elementType.GetMembers().Where(x => x.MemberType == MemberTypes.Property | x.MemberType == MemberTypes.NestedType | x.MemberType == MemberTypes.Field).ToList();

            //loop over each item in the list 
            foreach (var item in list)
            {
                try
                {
                    icount = 0;

                    foreach (DataColumn dc in tb.Columns)
                    {
                        var cnam = dc.ColumnName.ToLower();
                        var chk = (from p in cls where p.Name.Equals(cnam, StringComparison.OrdinalIgnoreCase) select p).ToList();

                        if (chk.Count > 0)
                        {
                            var c = props.Where(h => h.Name.Equals(cnam, StringComparison.OrdinalIgnoreCase)).First();
                            object val = null;
                            try
                            {
                                val = c.GetValue(item, null);
                            }
                            catch (Exception ex)
                            {
                            }

                            try
                            {
                                if (val == null)
                                {
                                    if (icount == tb.Columns.Count - 1)
                                    {
                                        qry2.AppendFormat("{0} = {1}", cnam, "NULL");
                                    }
                                    else
                                    {
                                        qry2.AppendFormat("{0} = {1}, ", cnam, "NULL");
                                    }

                                }
                                else if (c.PropertyType.Name.Equals("string", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (!val.IsDate())
                                    {
                                        val = ((string)val).RemoveSpecialCharacters(remove_xters);
                                    }


                                    if (icount == tb.Columns.Count - 1)
                                    {
                                        qry2.AppendFormat("{0} = '{1}' ", cnam, (val == null ? "NULL" : (val.IsDate() ? val.ToSQLDate() : val)));
                                    }
                                    else
                                    {
                                        qry2.AppendFormat("{0} = {1}, ", cnam, (val == null ? "NULL" : string.Format("'{0}'", (val.IsDate() ? val.ToSQLDate() : val))));
                                    }

                                }
                                else if (Xtenxion.IsNumeric(val) || c.PropertyType.FullName.ToLower().Contains("decimal")
                                || c.PropertyType.FullName.ToLower().Contains("int")
                                || c.PropertyType.FullName.ToLower().Contains("long")
                                || c.PropertyType.FullName.ToLower().Contains("double")
                                || c.PropertyType.FullName.ToLower().Contains("float"))
                                {

                                    if (icount == tb.Columns.Count - 1)
                                    {
                                        qry2.AppendFormat("{0} = {1}", cnam, (val == null ? 0 : val));
                                    }
                                    else
                                    {
                                        qry2.AppendFormat("{0} = {1},", cnam, (val == null ? 0 : val));
                                    }

                                }
                                else
                                {
                                    //tc = TypeCode.String Or tc = TypeCode.Char Or tc = TypeCode.Empty Or tc = TypeCode.Boolean Then
                                    //type is not numeric

                                    if (!val.IsDate() && !c.PropertyType.FullName.ToLower().Contains("date"))
                                    {
                                        val = ((string)val).RemoveSpecialCharacters(remove_xters);
                                    }

                                    if (icount == tb.Columns.Count - 1)
                                    {
                                        qry2.AppendFormat("{0} = '{1}' ", cnam, (val == null ? "NULL" : (val.IsDate() ? val.ToSQLDate() : val)));
                                    }
                                    else
                                    {
                                        qry2.AppendFormat("{0} = {1}, ", cnam, (val == null ? "NULL" : string.Format("'{0}'", (val.IsDate() ? val.ToSQLDate() : val))));
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.WriteErrorToEventLog(ex);
                            }
                        }
                        icount = icount + 1;
                    }

                    qry2.AppendFormat(string.Format(" WHERE {0} = {1}", IDColumnName, ID));


                    var strQuery =  qry2.ToString();

                    if (!string.IsNullOrEmpty(strQuery))
                    {
                        return strQuery;
                    }

                }
                catch (Exception ex)
                {
                    Logger.WriteErrorToEventLog(ex);
                }

            }

            return null;

        }
        public bool Update<T>(T obj, string TableName, string IDColumnName, string ID)
        {
            var strQuery = BuildUpdateQuery<T>(obj, TableName, IDColumnName, ID);

            if (!string.IsNullOrEmpty(strQuery))
            {
                var result = ExecuteQuery(strQuery);
                return result;
            }
            return false;
        }

        public List<T> Insert<T>(List<T> list, string TableName)
        {

            var result = new List<T>();
            var tb = DALExtensions.CollectionHelper.ConvertTo<T>(list);

            var qry1 = new StringBuilder();
            //start contruction of the insert part of the query
            qry1.AppendFormat("INSERT INTO {0} ", TableName);


            int icount = 0;
            qry1.Append("(");

            foreach (DataColumn x in tb.Columns)
            {
                if (icount == tb.Columns.Count - 1)
                {
                    qry1.AppendFormat("{0} ", x.ColumnName.ToUpper());
                }
                else
                {
                    qry1.AppendFormat("{0}, ", x.ColumnName.ToUpper());
                }
                icount = icount + 1;
            }

            qry1.Append(") ");

            Type elementType = typeof(T);
            var props = elementType.GetProperties();
            var cls = elementType.GetMembers().Where(x => x.MemberType == MemberTypes.Property | x.MemberType == MemberTypes.NestedType | x.MemberType == MemberTypes.Field).ToList();

            var queryz = new List<string>();

            //loop over each item in the list 

            foreach (var item in list)
            {

                try
                {
                    icount = 0;
                    var qry2 = new StringBuilder();

                    //start contruction of the value part of the query
                    qry2.AppendFormat(" VALUES (");

                    foreach (DataColumn dc in tb.Columns)
                    {
                        var cnam = dc.ColumnName.ToLower();
                        var chk = (from p in cls where p.Name.Equals(cnam, StringComparison.OrdinalIgnoreCase) select p).ToList();

                        if (chk.Count > 0)
                        {
                            var c = props.Where(h => h.Name.Equals(cnam, StringComparison.OrdinalIgnoreCase)).First();
                            object val = null;
                            try
                            {
                                val = c.GetValue(item, null);

                            }
                            catch (Exception ex)
                            {
                            }

                            try
                            {
                                if (val == null)
                                {
                                    if (icount == tb.Columns.Count - 1)
                                    {
                                        qry2.AppendFormat("{0} ", "NULL");
                                    }
                                    else
                                    {
                                        qry2.AppendFormat("{0}, ", "NULL");
                                    }

                                }
                                else if (c.PropertyType.Name.Equals("string", StringComparison.OrdinalIgnoreCase) )
                                {
                                    if (!val.IsDate())
                                    {
                                        val = ((string)val).RemoveSpecialCharacters(remove_xters);
                                    }

                                    if (icount == tb.Columns.Count - 1)
                                    {
                                        qry2.AppendFormat("{0} ", val == null ? "NULL" : string.Format("'{0}'", (val.IsDate() ? val.ToSQLDate() : val)));
                                    }
                                    else
                                    {
                                        qry2.AppendFormat("{0}, ", val == null ? "NULL" : string.Format("'{0}'", (val.IsDate() ? val.ToSQLDate() : val)));
                                    }

                                }
                                else if (Xtenxion.IsNumeric(val) || c.PropertyType.FullName.ToLower().Contains("decimal")
                                || c.PropertyType.FullName.ToLower().Contains("int")
                                || c.PropertyType.FullName.ToLower().Contains("long")
                                || c.PropertyType.FullName.ToLower().Contains("double")
                                || c.PropertyType.FullName.ToLower().Contains("float"))
                                {

                                    if (icount == tb.Columns.Count - 1)
                                    {
                                        qry2.AppendFormat("{0} ", val == null ? 0 : val);
                                    }
                                    else
                                    {
                                        qry2.AppendFormat("{0}, ", val == null ? 0 : val);
                                    }

                                }
                                else
                                {
                                    //tc = TypeCode.String Or tc = TypeCode.Char Or tc = TypeCode.Empty Or tc = TypeCode.Boolean Then
                                    //type is not numeric
                                    if (!val.IsDate() && !c.PropertyType.FullName.ToLower().Contains("date"))
                                    {
                                        val = ((string)val).RemoveSpecialCharacters(remove_xters);
                                    }

                                    if (icount == tb.Columns.Count - 1)
                                    {
                                        qry2.AppendFormat("{0} ", val == null ? "NULL" : string.Format("'{0}'", (val.IsDate() ? val.ToSQLDate() : val)));
                                    }
                                    else
                                    {
                                        qry2.AppendFormat("{0}, ", val == null ? "NULL" : string.Format("'{0}'", (val.IsDate() ? val.ToSQLDate() : val)));
                                    }
                                }
                            }
                            catch (Exception ex)
                            {

                            }

                        }
                        icount = icount + 1;
                    }

                    qry2.AppendFormat(")");


                    var strQuery = qry1.ToString() + qry2.ToString();


                    if (!string.IsNullOrEmpty(strQuery))
                    {
                        if (InvokeTxn)
                        {
                            queryz.Add(strQuery);
                            result.Add(item);
                        }
                        else
                        {
                            var flg = ExecuteQuery(strQuery);
                            if (flg)
                            {
                                result.Add(item);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteErrorToEventLog(ex);
                }

            }

            if (InvokeTxn)
            {
                var flg = ExecuteMany(queryz);
                if (!flg)
                {
                    result.Clear();
                }
            }

            return result;

        }

        public bool InsertRecord<T>(T obj, string TableName)
        {
            // Dim result = New List(Of T)
            var list = new List<T>();
            list.Add(obj);

            var dt = DALExtensions.CollectionHelper.ConvertTo<T>(list);
            try
            {
                var sqlQuery = "Select  * FROM " + TableName + " WHERE 0 = 1";
                var dataAdapter = new OleDbDataAdapter(sqlQuery, conStr);

                var ds = new DataSet();
                dataAdapter.FillSchema(ds, SchemaType.Source, TableName);
                dataAdapter.Fill(ds, TableName);

                Parallel.ForEach(dt.AsEnumerable(), (row) =>
               {
                   var newRow = ds.Tables[TableName].NewRow();
                   var tb = row.Table;
                   newRow = row;

                   ds.Tables[TableName].ImportRow(newRow);
               });

                OleDbCommandBuilder TempSqlCommandBuilder = new OleDbCommandBuilder(dataAdapter);

                if (InvokeTxn)
                {
                    Txn = dataAdapter.InsertCommand.Connection.BeginTransaction();
                    dataAdapter.InsertCommand.Transaction = Txn;
                    dataAdapter.UpdateCommand.Transaction = Txn;
                    dataAdapter.DeleteCommand.Transaction = Txn;
                }

                dataAdapter.Update(ds, TableName);
                Console.WriteLine("Records have been added! Check database to see changes");

                return true;

            }
            catch (Exception ex)
            {
                Logger.WriteErrorToEventLog(ex);
            }

            return false;


        }

        public bool InsertRecords<T>(List<T> list, string TableName, ref string errMsg)
        {

            var dt = DALExtensions.CollectionHelper.ConvertTo<T>(list);
            try
            {
                var sqlQuery = "SELECT  * FROM " + TableName + " WHERE 0 = 1";
                var dataAdapter = new OleDbDataAdapter(sqlQuery, conStr);
                var ds = new DataSet();
                dataAdapter.FillSchema(ds, SchemaType.Source, TableName);
                dataAdapter.Fill(ds, TableName);

                foreach (DataRow row in dt.Rows)
                {
                    var newRow = ds.Tables[TableName].NewRow();
                    var tb = row.Table;
                    newRow = row;

                    ds.Tables[TableName].ImportRow(newRow);
                }

                //foreach (DataRow row in dt.Rows)
                //{
                //    var newRow = ds.Tables[TableName].NewRow();
                //    var tb = row.Table;
                //    newRow = row;

                //    ds.Tables[TableName].ImportRow(newRow);
                //}

                OleDbCommandBuilder TempSqlCommandBuilder = new OleDbCommandBuilder(dataAdapter);

                if (InvokeTxn)
                {
                    Txn = dataAdapter.InsertCommand.Connection.BeginTransaction();
                    dataAdapter.InsertCommand.Transaction = Txn;
                    dataAdapter.UpdateCommand.Transaction = Txn;
                    dataAdapter.DeleteCommand.Transaction = Txn;
                }

                dataAdapter.Update(ds, TableName);

                Console.WriteLine("Records have been added! Check database to see changes");


                return true;

            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                Logger.WriteErrorToEventLog(ex);
            }

            return false;


        }

        public List<T> Insert<T>(List<T> list, string TableName, ref List<T> failed)
        {
            var result = new List<T>();
            var tb = DALExtensions.CollectionHelper.ConvertTo<T>(list);
            failed = new List<T>();

            var qry1 = new StringBuilder();
            //start contruction of the insert part of the query
            qry1.AppendFormat("INSERT INTO {0} ", TableName);


            int icount = 0;
            qry1.Append("(");


            foreach (DataColumn x in tb.Columns)
            {
                if (icount == tb.Columns.Count - 1)
                {
                    qry1.AppendFormat("{0} ", x.ColumnName.ToUpper());
                }
                else
                {
                    qry1.AppendFormat("{0}, ", x.ColumnName.ToUpper());
                }
                icount = icount + 1;
            }

            qry1.Append(") ");

            Type elementType = typeof(T);
            var props = elementType.GetProperties();
            var cls = elementType.GetMembers().Where(x => x.MemberType == MemberTypes.Property | x.MemberType == MemberTypes.NestedType | x.MemberType == MemberTypes.Field).ToList();

            var queryz = new List<string>();
            var failedList = new List<T>();
            //loop over each item in the list 
            //loop over each item in the list 
            Parallel.ForEach(list, item =>
             {
                 try
                 {
                     icount = 0;
                     var qry2 = new StringBuilder();

                     //start contruction of the value part of the query
                     qry2.AppendFormat(" VALUES (");


                     foreach (DataColumn dc in tb.Columns)
                     {
                         var cnam = dc.ColumnName.ToLower();
                         var chk = (from p in cls where p.Name.Equals(cnam, StringComparison.OrdinalIgnoreCase) select p).ToList();

                         if (chk != null)
                         {
                             if (chk.Count > 0)
                             {
                                 PropertyInfo c = null;

                                 try
                                 {
                                     var cList = props.Where(h => h.Name.Equals(cnam, StringComparison.OrdinalIgnoreCase)).ToList();

                                     if (cList != null)
                                     {
                                         if (cList.Count > 0)
                                         {
                                             c = cList.First();

                                             object val = null;
                                             try
                                             {
                                                 val = c.GetValue(item, null);

                                             }
                                             catch (Exception ex)
                                             {
                                             }

                                             try
                                             {
                                                 if (val == null)
                                                 {
                                                     if (icount == tb.Columns.Count - 1)
                                                     {
                                                         qry2.AppendFormat("{0} ", "NULL");
                                                     }
                                                     else
                                                     {
                                                         qry2.AppendFormat("{0}, ", "NULL");
                                                     }

                                                 }
                                                 else if (c.PropertyType.Name.Equals("string", StringComparison.OrdinalIgnoreCase) )
                                                 {
                                                     if (!val.IsDate())
                                                     {
                                                         val = ((string)val).RemoveSpecialCharacters(remove_xters);
                                                     }

                                                     if (icount == tb.Columns.Count - 1)
                                                     {
                                                         qry2.AppendFormat("{0} ", val == null ? "NULL" : string.Format("'{0}'", (val.IsDate() ? val.ToSQLDate() : val)));
                                                     }
                                                     else
                                                     {
                                                         qry2.AppendFormat("{0}, ", val == null ? "NULL" : string.Format("'{0}'", (val.IsDate() ? val.ToSQLDate() : val)));
                                                     }

                                                 }
                                                 else if (Xtenxion.IsNumeric(val) || c.PropertyType.FullName.ToLower().Contains("decimal")
                                || c.PropertyType.FullName.ToLower().Contains("int")
                                || c.PropertyType.FullName.ToLower().Contains("long")
                                || c.PropertyType.FullName.ToLower().Contains("double")
                                || c.PropertyType.FullName.ToLower().Contains("float"))
                                                 {

                                                     if (icount == tb.Columns.Count - 1)
                                                     {
                                                         qry2.AppendFormat("{0} ", val == null ? 0 : val);
                                                     }
                                                     else
                                                     {
                                                         qry2.AppendFormat("{0}, ", val == null ? 0 : val);
                                                     }

                                                 }
                                                 else
                                                 {
                                                     //tc = TypeCode.String Or tc = TypeCode.Char Or tc = TypeCode.Empty Or tc = TypeCode.Boolean Then
                                                     //type is not numeric
                                                     if (!val.IsDate() && !c.PropertyType.FullName.ToLower().Contains("date"))
                                                     {
                                                         val = ((string)val).RemoveSpecialCharacters(remove_xters);
                                                     }

                                                     if (icount == tb.Columns.Count - 1)
                                                     {
                                                         qry2.AppendFormat("{0} ", val == null ? "NULL" : string.Format("'{0}'", (val.IsDate() ? val.ToSQLDate() : val)));
                                                     }
                                                     else
                                                     {
                                                         qry2.AppendFormat("{0}, ", val == null ? "NULL" : string.Format("'{0}'", (val.IsDate() ? val.ToSQLDate() : val)));
                                                     }
                                                 }

                                             }
                                             catch (Exception ex)
                                             {
                                             }
                                         }
                                     }


                                 }
                                 catch (Exception ex)
                                 {
                                 }

                             }

                         }

                         if (icount == tb.Columns.Count - 1)
                         {
                             qry2.AppendFormat("{0} ", null);
                         }
                         else
                         {
                             qry2.AppendFormat("{0}, ", null);
                         }

                         icount = icount + 1;
                     }

                     var strqry2 = qry2.ToString().TrimEnd();
                     if (strqry2.Trim().EndsWith(","))
                     {
                         strqry2 = strqry2.Remove(strqry2.Length - 1, 1);
                     }
                     strqry2 = strqry2 + ")";

                     var strQuery = qry1.ToString() + strqry2;

                     if (!string.IsNullOrEmpty(strQuery))
                     {
                         if (InvokeTxn)
                         {
                             queryz.Add(strQuery);
                             result.Add(item);
                         }
                         else
                         {
                             var flg = ExecuteQuery(strQuery);
                             if (flg)
                             {
                                 result.Add(item);
                             }
                         }
                     }
                     else
                     {
                         failedList.Add(item);
                     }

                 }
                 catch (Exception ex)
                 {
                     Logger.WriteErrorToEventLog(ex);
                     failedList.Add(item);
                 }

             });

            if (InvokeTxn)
            {
                var flg = ExecuteMany(queryz);
                if (!flg)
                {
                    failed.AddRange(result);
                }

            }
            else
            {
                failed = failedList;
                failedList = null;
            }


            return result;

        }

    }

}

