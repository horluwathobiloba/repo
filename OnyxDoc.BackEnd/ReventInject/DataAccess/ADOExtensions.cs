using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Data.SqlClient;
using ReventInject.Utilities.Enums;
using System.Text;
using System.Reflection;
using System.Linq;
using System.Threading.Tasks;
using ReventInject.Utilities;
using ReventInject;

namespace ReventInject.DataAccess
{
    public class ADOExtensions
    {
        private string conStr;
        private SqlTransaction Txn;
        private bool InvokeTxn;
        private readonly ILogger _logger;

        public ADOExtensions(string _conStr, ILogger logger)
        {
            conStr = _conStr;
            _logger = logger;
        }

        public ADOExtensions(string _conStr, bool _InvokeTxn)
        {
            conStr = _conStr;
            InvokeTxn = _InvokeTxn;
        }

        public object ExecuteScalar(string StrQuery)
        {
            try
            {
                using (SqlConnection oracleConn = new SqlConnection(conStr))
                {
                    oracleConn.Open();
                    using (SqlCommand ocomd = new SqlCommand(StrQuery, oracleConn))
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
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                using (DataTable dt = new DataTable())
                {
                    var result = new List<T>();
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            conn.Open();
                            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                            dt.Load(dr);

                            if (dt != null)
                            {
                                result = DALExtensions.CollectionHelper.ConvertTo<T>(dt).ToList();
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        // handle error
                        result = new List<T>();
                        throw ex;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
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

            using (SqlConnection conn = new SqlConnection(conStr))
            {
                using (DataTable dt = new DataTable())
                {
                    var result = new DataTable();
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            conn.Open();
                            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                            dt.Load(dr);

                            if (dt != null)
                            {
                                result = dt;
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        // handle error
                        result = new DataTable();
                       throw ex;
                    }
                    catch (Exception ex)
                    {
                        // handle error
                        throw ex;
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

            using (SqlConnection conn = new SqlConnection(conStr))
            {
                var result = false;
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        var i = cmd.ExecuteNonQuery();

                        if (i > 0)
                        {
                            result = true;
                        }
                    }
                }
                catch (SqlException ex)
                {
                    // handle error
                   throw ex;
                }
                catch (Exception ex)
                {
                    // handle error
                    throw ex;
                }
                finally
                {
                    conn.Close();
                }

                return result;
            }
        }

        //public bool RecordExists(string query)
        //{
        //    try
        //    {
        //        return ExecuteQuery(query);

        //    }
        //    catch (SqlException ex)
        //    {
        //       throw ex;
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //        return false;
        //    }

        //}

        public bool ExecuteMany(List<string> queryStrings)
        {
            using (SqlConnection connection = new SqlConnection(conStr))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        if (InvokeTxn)
                        {
                            Txn = connection.BeginTransaction(IsolationLevel.ReadCommitted);
                            command.Transaction = Txn;
                        }

                        foreach (string x in queryStrings)
                        {
                            command.CommandText = x;
                            var count = command.ExecuteNonQuery();
                        }

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

                    throw ex;
                } 
            }
        }

        public bool RecordExists(string query)
        {
            try
            {
                using (SqlConnection oracleConn = new SqlConnection(conStr))
                {
                    oracleConn.Open();
                    using (SqlCommand ocomd = new SqlCommand(query, oracleConn))
                    {
                        object obj = ocomd.ExecuteScalar();

                        if (obj is DBNull || obj == null)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public bool ExecuteQuery(string queryString)
        {
            using (SqlConnection connection = new SqlConnection(conStr))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(queryString, connection))
                    {

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

                }
                catch (Exception ex)
                {
                    if (InvokeTxn)
                    {
                        Txn.Rollback();
                    } 
                    throw ex; 
                }
                return false;
            }
        }

        public bool Insert<T>(T obj, string TableName)
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
                                    if (!val.IsDate() && !c.PropertyType.FullName.ToLower().IN("date"))
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
                                Console.WriteLine(ex.Message);
                            }

                        }
                        icount = icount + 1;
                    }

                    qry2.AppendFormat(")");


                    var strQuery = qry1.ToString() + qry2.ToString();

                    if (!string.IsNullOrEmpty(strQuery))
                    {
                        var result = ExecuteQuery(strQuery);
                        return result;
                    }


                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            return false;

        }

        private char[] remove_xters = new char[] { ':', '*', '?', '"', '<', '>', '|', '/', '\'', '^', '{', '}', ',' };
        public bool Update<T>(T obj, string TableName, string IDColumnName, string ID)
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
                                throw ex;
                            }
                        }
                        icount = icount + 1;
                    }

                    qry2.AppendFormat(string.Format(" WHERE {0} = '{1}'", IDColumnName, ID));


                    var strQuery = qry2.ToString();

                    if (!string.IsNullOrEmpty(strQuery))
                    {
                        var result = ExecuteQuery(strQuery);
                        return result;
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
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
                    throw ex;
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
                var dataAdapter = new SqlDataAdapter(sqlQuery, conStr);

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

                SqlCommandBuilder TempSqlCommandBuilder = new SqlCommandBuilder(dataAdapter);

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
                throw ex;
            }

            return false;


        }

        public bool InsertRecords<T>(List<T> list, string TableName)
        {

            var dt = DALExtensions.CollectionHelper.ConvertTo<T>(list);
            try
            {
                var sqlQuery = "SELECT  * FROM " + TableName + " WHERE 0 = 1";
                var dataAdapter = new SqlDataAdapter(sqlQuery, conStr);
                var ds = new DataSet();
                dataAdapter.FillSchema(ds, SchemaType.Source, TableName);
                dataAdapter.Fill(ds, TableName);

                Parallel.ForEach(dt.AsEnumerable(), (row) =>
                    {
                        var newRow = ds.Tables[TableName].NewRow();
                        var tb = row.Table;
                        newRow = row;

                        ds.Tables[TableName].ImportRow(newRow);
                    }
               );

                //foreach (DataRow row in dt.Rows)
                //{
                //    var newRow = ds.Tables[TableName].NewRow();
                //    var tb = row.Table;
                //    newRow = row;

                //    ds.Tables[TableName].ImportRow(newRow);
                //}

                SqlCommandBuilder TempSqlCommandBuilder = new SqlCommandBuilder(dataAdapter);

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
                throw ex;
            }
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
                                                         qry2.AppendFormat("{0} ", null);
                                                     }
                                                     else
                                                     {
                                                         qry2.AppendFormat("{0}, ", null);
                                                     }
                                                     icount = icount + 1;
                                                     continue;
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
