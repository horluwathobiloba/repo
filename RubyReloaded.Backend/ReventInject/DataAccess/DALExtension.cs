using System;
using System.Collections.Generic;
using System.Data;
using System.ComponentModel;
using System.Reflection;
using System.Data.OleDb;
using System.Data.Common;
using System.Text;
using System.Data.SqlClient;
using System.Linq;
using ReventInject.Utilities;

namespace ReventInject.DataAccess
{
    public class DALExtension
    {
        private string constr;

        public DALExtension(string _constr)
        {
            constr = _constr;
        }

        public DataSet ToDataSet<T>(IList<T> list, string TableName)
        {

            Type elementType = typeof(T);
            DataSet ds = new DataSet();
            DataTable tb = new DataTable(TableName);
            ds.Tables.Add(tb);

            //add a column to table for each public property on T 
            foreach (PropertyInfo propInfo in elementType.GetProperties())
            {
                try
                {
                    var ty = propInfo.PropertyType;
                    Type utype = null;

                    if (ty.IsGenericType && (object.ReferenceEquals(ty.GetGenericTypeDefinition(), typeof(Nullable<>))))
                    {
                        utype = Nullable.GetUnderlyingType(ty.GetType());
                    }
                    tb.Columns.Add(propInfo.Name, utype);


                }
                catch (Exception ex)
                {
                }
            }

            //go through each property on T and add each value to the table 
            foreach (T item in list)
            {
                DataRow row = tb.NewRow();
                foreach (PropertyInfo propInfo in elementType.GetProperties())
                {
                    try
                    {
                        row[propInfo.Name] = propInfo.GetValue(item, null);
                    }
                    catch (Exception ex)
                    {
                    }
                }

                //This line was missing: 
                tb.Rows.Add(row);
            }

            tb.AcceptChanges();

            return ds;

        }

        public static bool IsNullableType(Type myType)
        {
            return (myType.IsGenericType) && (object.ReferenceEquals(myType.GetGenericTypeDefinition(), typeof(Nullable<>)));
        }

        public DataTable ToDataTable<T>(IList<T> list, string TableName)
        {

            Type elementType = typeof(T);
            DataSet ds = new DataSet();
            DataTable tb = new DataTable(TableName);
            ds.Tables.Add(tb);

            //add a column to table for each public property on T 
            foreach (PropertyInfo propInfo in elementType.GetProperties())
            {
                try
                {
                    var ty = propInfo.PropertyType;
                    Type utype = null;

                    if (ty.IsGenericType && (object.ReferenceEquals(ty.GetGenericTypeDefinition(), typeof(Nullable<>))))
                    {
                        utype = Nullable.GetUnderlyingType(ty);
                        tb.Columns.Add(propInfo.Name, utype);
                    }
                    else
                    {
                        tb.Columns.Add(propInfo.Name, ty);
                    }
                }
                catch (Exception ex)
                {
                }

            }

            //go through each property on T and add each value to the table 
            foreach (T item in list)
            {
                DataRow row = tb.NewRow();
                foreach (PropertyInfo propInfo in elementType.GetProperties())
                {
                    try
                    {
                        row[propInfo.Name] = propInfo.GetValue(item, null);
                    }
                    catch (Exception ex)
                    {
                    }
                }

                //This line was missing: 
                tb.Rows.Add(row);
            }

            tb.AcceptChanges();

            return tb;

        }

        public DataSet ToDataSet2<T>(IList<T> list, string TableName)
        {

            DataSet ds = new DataSet();

            var obj = Activator.CreateInstance<T>();

            SqlConnection dbConn = new SqlConnection(constr);
            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            var queryStr = "SELECT * FROM " + TableName + " WHERE 1 = 0";

            dataAdapter.SelectCommand = new SqlCommand(queryStr, dbConn);
            var dt = new DataTable();
            try
            {
                dbConn.Open();
                dataAdapter.Fill(dt);

            }
            catch (Exception ex)
            {
            }

            Type elementType = typeof(T);
            DataTable tb = new DataTable(TableName);

            var props = elementType.GetProperties();
            var cls = elementType.GetMembers().Where(x => x.MemberType == MemberTypes.Property | x.MemberType == MemberTypes.NestedType | x.MemberType == MemberTypes.Field).ToList();

            //add a column to table for each public property on T 

            foreach (MemberInfo memInfo in cls)
            {

                if (memInfo.MemberType == MemberTypes.Property | memInfo.MemberType == MemberTypes.Field)
                {
                    var pname = memInfo.Name;
                    var chk = (from DataColumn x in dt.Columns where x.ColumnName.Equals(pname, StringComparison.OrdinalIgnoreCase) select x).ToList();

                    if (chk.Count() > 0)
                    {
                        var c = props.Where(h => h.Name.Equals(pname, StringComparison.OrdinalIgnoreCase)).First();

                        try
                        {
                            var ty = c.PropertyType;
                            Type utype = null;
                            if (ty.IsGenericType && (object.ReferenceEquals(ty.GetGenericTypeDefinition(), typeof(Nullable<>))))
                            {
                                utype = Nullable.GetUnderlyingType(ty);
                                tb.Columns.Add(memInfo.Name, utype);
                            }
                            else
                            {
                                tb.Columns.Add(memInfo.Name, ty);
                            }

                        }
                        catch (Exception ex)
                        {
                        }

                    }

                }
            }

            //go through each property on T and add each value to the table 
            foreach (T item in list)
            {
                DataRow row = tb.NewRow();


                foreach (PropertyInfo propInfo in cls)
                {
                    if (propInfo.MemberType == MemberTypes.Property | propInfo.MemberType == MemberTypes.Field)
                    {
                        var pname = propInfo.Name;

                        var chk = (from DataColumn x in dt.Columns where x.ColumnName.Equals(pname, StringComparison.OrdinalIgnoreCase) select x).ToList();

                        if (chk.Count() > 0)
                        {
                            var c = props.Where(h => h.Name.Equals(pname, StringComparison.OrdinalIgnoreCase)).First();
                            try
                            {
                                row[propInfo.Name] = c.GetValue(item, null);

                            }
                            catch (Exception ex)
                            {
                            }
                        }

                    }
                }

                //This line was missing: 
                tb.Rows.Add(row);
            }

            tb.AcceptChanges();
            ds.Tables.Add(tb);

            return ds;
        }

        public DataTable ToDataTable2<T>(List<T> list, string TableName)
        {

            var obj = Activator.CreateInstance<T>();

            SqlConnection dbConn = new SqlConnection(constr);
            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            var queryStr = "SELECT * FROM " + TableName + " WHERE 1 = 0";

            dataAdapter.SelectCommand = new SqlCommand(queryStr, dbConn);
            var dt = new DataTable();
            try
            {
                dbConn.Open();
                dataAdapter.Fill(dt);

            }
            catch (Exception ex)
            {
            }


            Type elementType = typeof(T);
            DataTable tb = new DataTable(TableName);

            var props = elementType.GetProperties();
            var memtypes = elementType.GetMembers().Where(x => x.MemberType == MemberTypes.Property | x.MemberType == MemberTypes.NestedType | x.MemberType == MemberTypes.Field).ToList();

            //add a column to table for each public property on T 

            foreach (MemberInfo memInfo in memtypes)
            {
                if (memInfo.MemberType == MemberTypes.Property | memInfo.MemberType == MemberTypes.Field)
                {
                    var pname = memInfo.Name;
                    var chk = (from DataColumn x in dt.Columns where x.ColumnName.Equals(pname, StringComparison.OrdinalIgnoreCase) select x).ToList();

                    if (chk.Count() > 0)
                    {
                        var c = props.Where(h => h.Name.Equals(pname, StringComparison.OrdinalIgnoreCase)).First();
                        try
                        {
                            var ty = c.PropertyType;
                            Type utype = null;
                            if (ty.IsGenericType && (object.ReferenceEquals(ty.GetGenericTypeDefinition(), typeof(Nullable<>))))
                            {
                                utype = Nullable.GetUnderlyingType(ty);
                                tb.Columns.Add(memInfo.Name, utype);
                            }
                            else
                            {
                                tb.Columns.Add(memInfo.Name, ty);
                            }

                        }
                        catch (Exception ex)
                        {
                        }
                    }

                }
            }

            //go through each property on T and add each value to the table 
            foreach (T item in list)
            {
                DataRow row = tb.NewRow();


                foreach (MemberInfo propInfo in memtypes)
                {
                    if (propInfo.MemberType == MemberTypes.Property | propInfo.MemberType == MemberTypes.Field)
                    {
                        var pname = propInfo.Name;
                        var chk = (from DataColumn x in dt.Columns where x.ColumnName.Equals(pname, StringComparison.OrdinalIgnoreCase) select x).ToList();

                        if (chk.Count() > 0)
                        {
                            var c = props.Where(h => h.Name.Equals(pname, StringComparison.OrdinalIgnoreCase)).First();
                            try
                            {
                                row[propInfo.Name] = c.GetValue(item, null);

                            }
                            catch (Exception ex)
                            {
                            }
                        }

                    }
                }

                //This line was missing: 
                tb.Rows.Add(row);
            }

            tb.AcceptChanges();
            return tb;

        }

    }

    public class CollectionHelper
    {
        #region "Generic Converter"

        public DataTable ConvertTo<T>(IList<T> list)
        {
            DataTable table = CreateTable<T>();
            Type entityType = typeof(T);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (T item in list)
            {
                DataRow row = table.NewRow();

                foreach (PropertyDescriptor prop in properties)
                {
                    try
                    {
                        row[prop.Name] = prop.GetValue(item);

                    }
                    catch (Exception ex)
                    {
                    }
                }

                table.Rows.Add(row);
            }

            return table;
        }

        public IList<T> ConvertTo<T>(IList<DataRow> rows)
        {
            IList<T> list = null;

            if ((rows != null))
            {
                list = new List<T>();
                try
                {
                    foreach (DataRow row in rows)
                    {
                        try
                        {
                            T item = CreateItem<T>(row);
                            list.Add(item);

                        }
                        catch (Exception ex)
                        {
                        }
                    }

                }
                catch (Exception ex)
                {
                }
            }

            return list;
        }

        public IList<T> ConvertTo<T>(DataTable table)
        {
            if (table == null)
            {
                return null;
            }

            List<DataRow> rows = new List<DataRow>();

            foreach (DataColumn tn in table.Columns)
            {
                tn.ColumnName = tn.ColumnName.Trim();
            }

            table.AcceptChanges();

            foreach (DataRow row in table.Rows)
            {
                try
                {
                    rows.Add(row);

                }
                catch (Exception ex)
                {
                }
            }
            var a = ConvertTo<T>(rows);
            return a;
        }

        private object DoConvert<T>(object Value)
        {
            var result = (T)Value;

            return result;
        }

        public T CreateItem<T>(DataRow row)
        {
            T obj = default(T);
            if ((row != null))
            {
                obj = Activator.CreateInstance<T>();

                var i = 0;
                PropertyInfo[] props = obj.GetType().GetProperties();

                foreach (DataColumn column in row.Table.Columns)
                {

                    try
                    {
                        var cname = column.ColumnName;
                        var prop = (from x in props where x.Name.ToLower().Trim() == cname.ToLower().Trim() select x).First();

                        try
                        {
                            // This was a major issue for me, but God saw me thru it.
                            var ty = prop.PropertyType;
                            if (ty.IsGenericType && (object.ReferenceEquals(ty.GetGenericTypeDefinition(), typeof(Nullable<>))))
                            {
                                ty = Nullable.GetUnderlyingType(ty);
                            }
                            //OR 
                            //Dim ty As Type = Type.GetType(ans.PropertyType.Name)
                            //Dim value As Object = Convert.ChangeType(row(column.ColumnName), ty)

                            if (row[prop.Name] == null | System.DBNull.Value == row[prop.Name])
                            {
                                continue;
                            }
                            else
                            {
                                object value = null;
                                try
                                {
                                    value = ((IConvertible)row[prop.Name]).ToType(ty, null);

                                }
                                catch (Exception ex)
                                {
                                }

                                if (value == null)
                                {
                                    continue;
                                }
                                else
                                {
                                    if (prop.PropertyType.Name.Equals("decimal", StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        if (value.GetType().Name.Equals("double", StringComparison.InvariantCultureIgnoreCase)
                                            || value.GetType().Name.Equals("float", StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            prop.SetValue(obj, System.Convert.ToDecimal(value), null);
                                        }
                                        else
                                        {
                                            prop.SetValue(obj, value, null);
                                        }
                                    }
                                    else
                                    {
                                        prop.SetValue(obj, value, null);
                                    }
                                }

                                i += 1;
                            }

                        }
                        catch
                        {
                        }


                    }
                    catch (Exception ex)
                    {
                    }
                }
            }

            return obj;

        }

        public DataTable CreateTable<T>()
        {
            Type entityType = typeof(T);
            DataTable table = new DataTable(entityType.Name);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            try
            {
                foreach (PropertyDescriptor prop in properties)
                {
                    var ty = prop.PropertyType;
                    Type utype = null;

                    if (ty.IsGenericType && (object.ReferenceEquals(ty.GetGenericTypeDefinition(), typeof(Nullable<>))))
                    {
                        utype = Nullable.GetUnderlyingType(ty);
                        table.Columns.Add(prop.Name, utype);
                    }
                    else
                    {
                        table.Columns.Add(prop.Name, ty);
                    }

                }
            }
            catch (Exception ex)
            {
                table = null;
            }

            return table;
        }

        #endregion

        #region "Advance Generic Converters"

        public T CreateObj<T>(DataRow row, T obj = default(T))
        {

            if (obj == null)
            {
                obj = Activator.CreateInstance<T>();
            }
            if ((row != null))
            {
                var props = obj.GetType().GetProperties();
                var cls = obj.GetType().GetMembers().Where(x => x.MemberType == MemberTypes.Property | x.MemberType == MemberTypes.NestedType | x.MemberType == MemberTypes.Field).ToList();

                foreach (PropertyInfo p in cls)
                {
                    var pname = p.Name;
                    switch (p.MemberType)
                    {

                        case MemberTypes.Property:
                        case MemberTypes.Field:
                            var chk = from DataColumn x in row.Table.Columns where x.ColumnName.ToLower().Trim() == pname.ToLower().Trim() select x;

                            try
                            {
                                var c = props.Where(h => h.Name.Equals(pname, StringComparison.OrdinalIgnoreCase)).First();

                                var ty = p.PropertyType;
                                var p_value = c.GetValue(obj, null);
                                // get the property and its values as an object


                                try
                                {
                                    if (chk.Count() > 0)
                                    {
                                        Console.WriteLine("property name found, {0}", p.Name);
                                        object value = ((IConvertible)row[p.Name]).ToType(ty, null);

                                        if (value == null)
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            if (p.PropertyType.Name.Equals("decimal", StringComparison.InvariantCultureIgnoreCase))
                                            {
                                                if (value.GetType().Name.Equals("double", StringComparison.InvariantCultureIgnoreCase)
                                                    || value.GetType().Name.Equals("float", StringComparison.InvariantCultureIgnoreCase))
                                                {
                                                    p.SetValue(obj, System.Convert.ToDecimal(value), null);
                                                }
                                                else
                                                {
                                                    p.SetValue(obj, value, null);
                                                }
                                            }
                                            else
                                            {
                                                p.SetValue(obj, value, null);
                                            }
                                        }

                                        //result.Add(p)
                                    }
                                    else
                                    {
                                        if (p_value == null)
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            var i = p.PropertyType.Assembly.GetName().Name;
                                            var j = obj.GetType().Assembly.GetName().Name;

                                            if (i == j)
                                            {
                                                var value = CreateObj(row, p_value);

                                                if (value == null)
                                                {
                                                    continue;
                                                }
                                                else
                                                {
                                                    if (p.PropertyType.Name.Equals("decimal", StringComparison.InvariantCultureIgnoreCase))
                                                    {
                                                        if (value.GetType().Name.Equals("double", StringComparison.InvariantCultureIgnoreCase)
                                                            || value.GetType().Name.Equals("float", StringComparison.InvariantCultureIgnoreCase))
                                                        {
                                                            p.SetValue(obj, System.Convert.ToDecimal(value), null);
                                                        }
                                                        else
                                                        {
                                                            p.SetValue(obj, value, null);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        p.SetValue(obj, value, null);
                                                    }
                                                }

                                                Console.WriteLine("Found same Assembly! for {0} and {1}", p.Name, obj.GetType().Name);
                                            }
                                            else
                                            {
                                                Console.WriteLine("Assembly not same for {0} and {1}", p.Name, obj.GetType().Name);
                                            }

                                        }
                                    }


                                }
                                catch (Exception ex)
                                {
                                }


                            }
                            catch (Exception ex)
                            {
                            }
                            break;
                        case (MemberTypes.NestedType):

                            var ntProps = p.ReflectedType.GetProperties();

                            foreach (PropertyInfo q in ntProps)
                            {
                                var nam = q.Name;
                                var chkk = (from DataColumn x in row.Table.Columns where x.ColumnName.ToLower() == nam.ToLower() select x).ToList();

                                var ty = q.PropertyType;
                                var p_value = q.GetValue(obj, null);
                                // get the property and its values as an object

                                try
                                {
                                    if (chkk.Count() > 0)
                                    {
                                        Console.WriteLine("property name found, {0}", q.Name);
                                        object value = ((IConvertible)row[q.Name]).ToType(ty, null);

                                        if (value == null)
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            if (p.PropertyType.Name.Equals("decimal", StringComparison.InvariantCultureIgnoreCase))
                                            {
                                                if (value.GetType().Name.Equals("double", StringComparison.InvariantCultureIgnoreCase)
                                                    || value.GetType().Name.Equals("float", StringComparison.InvariantCultureIgnoreCase))
                                                {
                                                    p.SetValue(obj, System.Convert.ToDecimal(value), null);
                                                }
                                                else
                                                {
                                                    p.SetValue(obj, value, null);
                                                }
                                            }
                                            else
                                            {
                                                p.SetValue(obj, value, null);
                                            }
                                        }

                                    }
                                    else
                                    {
                                        if (p_value == null)
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            var i = p.PropertyType.Assembly.GetName().Name;
                                            var j = obj.GetType().Assembly.GetName().Name;

                                            if (i == j)
                                            {
                                                var value = CreateObj(row, p_value);

                                                if (value == null)
                                                {
                                                    continue;
                                                }
                                                else
                                                {
                                                    if (p.PropertyType.Name.Equals("decimal", StringComparison.InvariantCultureIgnoreCase))
                                                    {
                                                        if (value.GetType().Name.Equals("double", StringComparison.InvariantCultureIgnoreCase)
                                                            || value.GetType().Name.Equals("float", StringComparison.InvariantCultureIgnoreCase))
                                                        {
                                                            p.SetValue(obj, System.Convert.ToDecimal(value), null);
                                                        }
                                                        else
                                                        {
                                                            p.SetValue(obj, value, null);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        p.SetValue(obj, value, null);
                                                    }
                                                }
                                                Console.WriteLine("Found same Assembly! for {0} and {1}", p.Name, obj.GetType().Name);
                                            }
                                            else
                                            {
                                                Console.WriteLine("Assembly not same for {0} and {1}", p.Name, obj.GetType().Name);
                                            }
                                        }
                                    }


                                }
                                catch (Exception ex)
                                {
                                    Logger.WriteErrorToEventLog(ex);
                                }
                            }

                            break;
                    }

                }
            }


            return obj;

        }

        public IList<T> ConvertToObjList<T>(IList<DataRow> rows)
        {
            IList<T> list = null;

            if ((rows != null))
            {
                list = new List<T>();

                try
                {
                    foreach (DataRow row in rows)
                    {
                        T item = CreateObj<T>(row, default(T));
                        list.Add(item);
                    }



                }
                catch (Exception ex)
                {
                }
            }

            return list;
        }

        public IList<T> ConvertToObjList<T>(DataTable table)
        {
            if (table == null)
            {
                return null;
            }

            List<DataRow> rows = new List<DataRow>();

            foreach (DataRow row in table.Rows)
            {
                rows.Add(row);
            }
            var a = ConvertToObjList<T>(rows);
            return a;
        }

        #endregion


        #region "Generic ADO Inserts"

        public DataTable getTableStructure(string TableName, string constr)
        {
            var conn = new OleDbConnection(constr);
            var cmd = new OleDbCommand();

            var query = "SELECT * FROM " + TableName + " WHERE ROWNUM  = 0";

            cmd.Connection = conn;
            DataTable tbl = new DataTable();


            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Open();
                cmd = new OleDbCommand(query, conn);
                OleDbDataAdapter adap = new OleDbDataAdapter(cmd);
                //executing the command and assigning it to connection
                adap.Fill(tbl);

            }
            catch (Exception ex)
            {
                tbl = null;

            }
            finally
            {
                conn.Dispose();
            }

            return tbl;

        }

        public List<PropInfo> getObjectInfo<T>(DataTable tbl, T ob)
        {
            var result = new List<PropInfo>();
            var props = ob.GetType().GetProperties();
            var cls = ob.GetType().GetMembers().Where(x => x.MemberType == MemberTypes.Property | x.MemberType == MemberTypes.NestedType | x.MemberType == MemberTypes.Field).ToList();


            foreach (MemberInfo c in cls)
            {
                var cname = c.Name;
                switch (c.MemberType)
                {

                    case MemberTypes.Property:
                    case MemberTypes.Field:

                        var chk = from DataColumn x in tbl.Columns where x.ColumnName.Equals(cname, StringComparison.OrdinalIgnoreCase) select x;

                        var p = props.Where(h => h.Name.Equals(cname, StringComparison.OrdinalIgnoreCase)).First();
                        var ty = p.PropertyType;
                        var p_value = p.GetValue(ob, null);

                        try
                        {

                            if (chk.Count() > 0)
                            {
                                Console.WriteLine("property name found, {0} Vlaue {1}", p.Name, p_value);
                                result.Add(new PropInfo
                                {
                                    NameKey = p.Name,
                                    Value = p_value
                                });
                            }
                            else
                            {
                                var i = p.PropertyType.Assembly.GetName().Name;
                                var j = ob.GetType().Assembly.GetName().Name;

                                if (i == j)
                                {
                                    if (p_value == null)
                                    {
                                        continue;
                                    }
                                    result.AddRange(getObjectInfo(tbl, p_value));
                                }
                                else
                                {
                                    continue;
                                }

                            }


                        }
                        catch (Exception ex)
                        {
                            Logger.WriteErrorToEventLog(ex);
                        }

                        break;
                    case MemberTypes.NestedType:

                        var ntProps = c.ReflectedType.GetProperties();

                        foreach (PropertyInfo px in ntProps)
                        {
                            var name = px.Name;
                            var chkk = from DataColumn x in tbl.Columns where x.ColumnName.ToLower() == name.ToLower() select x;

                            var tyy = px.PropertyType;
                            var pp_value = px.GetValue(ob, null);
                            // get the property and its values as an object

                            try
                            {
                                if (chkk.Count() > 0)
                                {
                                    Console.WriteLine("property name found: {0}, Vlaue: {1}", px.Name, pp_value);
                                    result.Add(new PropInfo
                                    {
                                        NameKey = px.Name,
                                        Value = pp_value
                                    });
                                }
                                else
                                {
                                    var i = px.PropertyType.Assembly.GetName().Name;
                                    var j = ob.GetType().Assembly.GetName().Name;

                                    if (i == j)
                                    {
                                        if (pp_value == null)
                                        {
                                            continue;
                                        }
                                        result.AddRange(getObjectInfo(tbl, pp_value));
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }


                            }
                            catch (Exception ex)
                            {
                            }
                        }

                        break;
                }


            }

            return result;

        }

        public List<PropInfo> getObjectInfo<T>(List<string> NameList, T ob)
        {
            var result = new List<PropInfo>();
            var props = ob.GetType().GetProperties();
            var cls = ob.GetType().GetMembers().Where(x => x.MemberType == MemberTypes.Property | x.MemberType == MemberTypes.NestedType | x.MemberType == MemberTypes.Field).ToList();

            foreach (MemberInfo c in cls)
            {
                switch (c.MemberType)
                {
                    case MemberTypes.Property:
                    case MemberTypes.Field:
                        var name = c.Name;
                        var chk = from x in NameList where x.ToLower() == name.ToLower() select x;

                        var p = props.Where(h => h.Name == name).First();
                        var ty = p.PropertyType;
                        var p_value = p.GetValue(ob, null);

                        try
                        {

                            if (chk.Count() > 0)
                            {
                                Console.WriteLine("property name found, {0} Vlaue {1}", p.Name, p_value);
                                result.Add(new PropInfo
                                {
                                    NameKey = p.Name,
                                    Value = p_value
                                });
                            }
                            else
                            {
                                var i = p.PropertyType.Assembly.GetName().Name;
                                var j = ob.GetType().Assembly.GetName().Name;

                                if (i == j)
                                {
                                    if (p_value == null)
                                    {
                                        continue;
                                    }
                                    result.AddRange(getObjectInfo(NameList, p_value));
                                }
                                else
                                {
                                    continue;
                                }
                            }


                        }
                        catch (Exception ex)
                        {
                            Logger.WriteErrorToEventLog(ex);
                        }
                        break;
                    case (MemberTypes.NestedType):

                        var ntProps = c.ReflectedType.GetProperties();

                        foreach (PropertyInfo xp in ntProps)
                        {
                            var Name = xp.Name;
                            var Chk = from x in NameList where x.ToLower() == Name.ToLower() select x;

                            var Ty = xp.PropertyType;
                            var P_value = xp.GetValue(ob, null);
                            // get the property and its values as an object

                            try
                            {
                                if (Chk.Count() > 0)
                                {
                                    Console.WriteLine("property name found: {0}, Vlaue: {1}", xp.Name, P_value);
                                    result.Add(new PropInfo
                                    {
                                        NameKey = xp.Name,
                                        Value = P_value
                                    });
                                }
                                else
                                {
                                    var i = xp.PropertyType.Assembly.GetName().Name;
                                    var j = ob.GetType().Assembly.GetName().Name;

                                    if (i == j)
                                    {
                                        if (P_value == null)
                                        {
                                            continue;
                                        }
                                        result.AddRange(getObjectInfo(NameList, P_value));
                                    }
                                    else
                                    {
                                        continue;
                                    }

                                }

                            }
                            catch (Exception ex)
                            {
                            }
                        }

                        break;
                }


            }

            return result;

        }

        public List<PropertyInfo> getPropertyInfo<T>(DataRow row, T obj = default(T))
        {
            var result = new List<PropertyInfo>();

            obj = Activator.CreateInstance<T>();
            var props = obj.GetType().GetProperties();
            foreach (PropertyInfo p in props)
            {
                var name = p.Name;
                var chk = from DataColumn x in row.Table.Columns where x.ColumnName.ToLower() == name.ToLower() select x;

                var ty = p.PropertyType;
                var p_value = p.GetValue(obj, null);
                // get the property and its values as an object


                try
                {
                    if (chk.Count() > 0)
                    {
                        Console.WriteLine("property name found, {0}", p.Name);
                        result.Add(p);

                    }
                    else
                    {
                        var i = p.PropertyType.Assembly.GetName().Name;
                        var j = obj.GetType().Assembly.GetName().Name;

                        if (i == j)
                        {
                            if (p_value == null)
                            {
                                continue;
                            }
                            result.AddRange(getPropertyInfo(row, p_value));
                        }
                        else
                        {
                            continue;
                        }

                    }


                }
                catch (Exception ex)
                {
                }

            }

            return result;

        }


        public List<PropertyInfo> getPropertyInfo<T>(string TableName, string constr, T obj = default(T))
        {
            var result = new List<PropertyInfo>();
            var tbl = getTableStructure(TableName, constr);

            var props = obj.GetType().GetProperties();
            foreach (PropertyInfo p in props)
            {
                var name = p.Name;
                var chk = from DataColumn x in tbl.Columns where x.ColumnName.ToLower() == name.ToLower() select x;

                var ty = p.PropertyType;
                var p_value = p.GetValue(obj, null);
                // get the property and its values as an object


                try
                {
                    if (chk.Count() > 0)
                    {
                        Console.WriteLine("property name found, {0}", p.Name);
                        result.Add(p);
                    }
                    else
                    {
                        var i = p.PropertyType.Assembly.GetName().Name;
                        var j = obj.GetType().Assembly.GetName().Name;

                        if (i == j)
                        {
                            if (p_value == null)
                            {
                                continue;
                            }
                            result.AddRange(getPropertyInfo(TableName, constr, p_value));
                        }
                        else
                        {
                            continue;
                        }
                    }


                }
                catch (Exception ex)
                {
                }

            }

            return result;

        }

        public Dictionary<string, object> getPropertyInfos<T>(DataTable tbl, T ob)
        {
            var result = new Dictionary<string, object>();

            var props = ob.GetType().GetProperties();


            foreach (PropertyInfo p in props)
            {
                var name = p.Name;
                var chk = from DataColumn x in tbl.Columns where x.ColumnName.ToLower() == name.ToLower() select x;

                var ty = p.PropertyType;
                var p_value = p.GetValue(ob, null);
                // get the property and its values as an object

                try
                {
                    if (chk.Count() > 0)
                    {
                        Console.WriteLine("property name found, {0}", p.Name);
                        result.Add(p.Name, p_value);

                    }
                    else
                    {
                        var i = p.PropertyType.Assembly.GetName().Name;
                        var j = ob.GetType().Assembly.GetName().Name;

                        if (i == j)
                        {
                            if (p_value == null)
                            {
                                continue;
                            }
                            var col = getPropertyInfos(tbl, p_value);
                            foreach (KeyValuePair<string, object> x in col)
                            {
                                result.Add(x.Key, x.Value);
                            }
                        }
                        else
                        {
                            continue;
                        }

                    }


                }
                catch (Exception ex)
                {
                }
            }

            return result;

        }

        public Dictionary<string, object> getPropertyInfos<T>(List<string> NameList, T ob)
        {
            var result = new Dictionary<string, object>();

            var props = ob.GetType().GetProperties();


            foreach (PropertyInfo p in props)
            {
                var name = p.Name;
                var chk = from x in NameList where x.ToLower() == name.ToLower() select x;

                var ty = p.PropertyType;
                var p_value = p.GetValue(ob, null);
                // get the property and its values as an object


                try
                {
                    if (chk.Count() > 0)
                    {
                        Console.WriteLine("property name found, {0}", p.Name);
                        result.Add(p.Name, p_value);

                    }
                    else
                    {
                        var i = p.PropertyType.Assembly.GetName().Name;
                        var j = ob.GetType().Assembly.GetName().Name;

                        if (i == j)
                        {
                            if (p_value == null)
                            {
                                continue;
                            }
                            var col = getPropertyInfos(NameList, p_value);
                            foreach (KeyValuePair<string, object> x in col)
                            {
                                result.Add(x.Key, x.Value);
                            }
                        }
                        else
                        {
                            continue;
                        }

                    }

                }
                catch (Exception ex)
                {
                }
            }

            return result;

        }

        #region "Inserts"

        public bool InsertObject<T>(DataTable Tbl, T obj, string constr)
        {

            var result = false;

            var conn = new OleDbConnection(constr);
            var cmd = new OleDbCommand();
            var cnames = new StringBuilder(string.Empty);
            var values = new StringBuilder(string.Empty);

            var props = getObjectInfo<T>(Tbl, obj);

            var vals = new StringBuilder(string.Empty);
            var cols = new StringBuilder(string.Empty);

            try
            {

                if (props.Count > 0)
                {
                    foreach (PropInfo prop in props)
                    {
                        try
                        {
                            if (prop.Value == null)
                            {
                                continue;
                            }
                            else
                            {
                                var a = prop.Value.GetType();

                                cnames.AppendFormat("@{0},", prop.NameKey);
                                values.AppendFormat("'{0}',", prop.Value);

                                if (Xtenxion.IsDate(prop.Value) | a.Name.ToLower() == "date" | a.Name.ToLower() == "datetime")
                                {
                                    var dt = string.Format("{0}", string.Format("{0:dd-MMM-yyyy}", (DateTime)prop.Value));
                                    cols.AppendFormat("{0},", prop.NameKey);
                                    vals.AppendFormat("TO_DATE(@{0},'DD/MM/YYYY'),", prop.NameKey);
                                    cmd.Parameters.AddWithValue("@" + prop.NameKey, dt);
                                }
                                else
                                {
                                    cols.AppendFormat("{0},", prop.NameKey);
                                    vals.AppendFormat("@{0},", prop.NameKey);
                                    cmd.Parameters.AddWithValue("@" + prop.NameKey, prop.Value);
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            obj = default(T);
                        }

                    }

                }

                //Add SqlParameters to the SqlCommand   

                var cn = cols.ToString().TrimEnd(' ').TrimEnd(',');
                var vl = vals.ToString().TrimEnd(' ').TrimEnd(',');

                var col = cols.ToString().TrimEnd(' ').TrimEnd(',');
                var val = vals.ToString().TrimEnd(' ').TrimEnd(',');

                var query = string.Format("INSERT INTO {0} ({1}) VALUES ({2})", Tbl.TableName, col, val);


                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Open();

                cmd = new OleDbCommand(query, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                //Execute the query.   
                var i = 0;
                i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    result = true;
                }


            }
            catch (Exception ex)
            {
                Logger.WriteErrorToEventLog(ex);
            }

            return result;

        }

        public bool InsertObject<T>(string TableName, T obj, string constr)
        {

            var result = false;

            var conn = new OleDbConnection(constr);
            var cmd = new OleDbCommand();
            var col_names = string.Empty;
            var values = string.Empty;
            var tbl = getTableStructure(TableName, constr);

            var props = getObjectInfo<T>(tbl, obj);

            var vals = new StringBuilder(string.Empty);
            var cols = new StringBuilder(string.Empty);

            try
            {

                if (props.Count > 0)
                {
                    foreach (PropInfo prop in props)
                    {
                        try
                        {
                            if (prop.Value == null)
                            {
                                continue;
                            }
                            else
                            {
                                //col_names = col_names & prop.NameKey & ','
                                //values = values & prop.Value & ','
                                var a = prop.Value.GetType();
                                if (cols.ToString().Contains(prop.NameKey))
                                {
                                    continue;

                                }
                                else
                                {
                                    if (Xtenxion.IsDate(prop.Value) | a.Name.ToLower() == "date" | a.Name.ToLower() == "dateTime")
                                    {
                                        var dt = string.Format("{0}", string.Format("{0:dd-MMM-yyyy|", prop.Value));
                                        cols.AppendFormat("{0},", prop.NameKey);
                                        vals.AppendFormat("TO_DATE('{0}','DD/MM/YYYY'),", dt);
                                    }
                                    else
                                    {
                                        cols.AppendFormat("{0},", prop.NameKey);
                                        vals.AppendFormat("'{0}',", prop.Value);
                                    }
                                }
                            }

                        }
                        catch
                        {
                            obj = default(T);
                        }

                    }

                }

                //Add SqlParameters to the SqlCommand   
                col_names.TrimEnd(',');
                values.TrimEnd(',');

                var col = cols.ToString().TrimEnd(' ').TrimEnd(',');
                var val = vals.ToString().TrimEnd(' ').TrimEnd(',');

                //Dim query = "INSERT INTO " & TableName & " (" & col_names & ")  VALUES (" & values & ")"
                var query = string.Format("INSERT INTO {0} ({1}) VALUES ({2})", TableName, col, val);

                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Open();

                //Execute the query.   
                var i = 0;
                cmd = new OleDbCommand(query, conn);
                //cmd.CommandType = CommandType.Text
                i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    result = true;
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorToEventLog(ex);
            }

            return result;

        }

        public bool InsertRecord<T>(string TableName, DataRow row, string constr)
        {

            var result = false;
            // Define a connection object
            var Conn = new OleDbConnection(constr);

            if (Conn.State == ConnectionState.Open)
            {
                Conn.Close();
            }
            Conn.Open();

            // Create a data adapter to retrieve records from db
            OleDbDataAdapter daUsers = new OleDbDataAdapter("SELECT * FROM " + TableName + " WHERE ROWNUM  = 0", Conn);
            DataSet dsUsers = new DataSet(TableName);

            daUsers.Fill(dsUsers, TableName);
            DataRow r = dsUsers.Tables[TableName].NewRow();

            var props = getPropertyInfo<T>(row, default(T));
            var colnames = new List<string>();
            foreach (PropertyInfo prop in props)
            {

                if (prop != null)
                {
                    //check if the column name has already been added
                    if (colnames.Contains(prop.Name))
                    {
                        continue;
                    }

                    try
                    {
                        var ty = prop.PropertyType;
                        if (ty.IsGenericType && (object.ReferenceEquals(ty.GetGenericTypeDefinition(), typeof(Nullable<>))))
                        {
                            ty = Nullable.GetUnderlyingType(ty);
                        }
                        object value = ((IConvertible)row[prop.Name]).ToType(ty, null);
                        r[prop.Name] = value;
                        colnames.Add(prop.Name);

                    }
                    catch
                    {
                    }
                }

            }

            try
            {
                daUsers.TableMappings.Add(TableName, TableName);
                dsUsers.Tables[0].Rows.Add(r);
                // add the object
                daUsers.Update(dsUsers.GetChanges(), TableName);
                // Insert the record even in the database
                dsUsers.AcceptChanges();
                // Align in-memory data with the data source ones
                colnames = null;
                result = true;

            }
            catch (Exception ex)
            {
                dsUsers.RejectChanges();
                // Reject DataSet changes
                Logger.WriteErrorToEventLog(ex);
            }

            return result;

        }

        public bool InsertRecord<T>(string TableName, T obj, string constr)
        {

            var result = false;
            // Define a connection object
            var Conn = new OleDbConnection(constr);
            var tbl = getTableStructure(TableName, constr);

            if (Conn.State == ConnectionState.Open)
            {
                Conn.Close();
            }
            Conn.Open();

            // Create a data adapter to retrieve records from db
            OleDbDataAdapter daUsers = new OleDbDataAdapter("SELECT * FROM " + TableName + " WHERE ROWNUM  = 0", Conn);
            DataSet dsUsers = new DataSet(TableName);

            daUsers.Fill(dsUsers, TableName);
            DataRow r = dsUsers.Tables[TableName].NewRow();

            var props = getObjectInfo<T>(tbl, obj);
            var colnames = new List<string>();

            foreach (PropInfo prop in props)
            {

                if (prop != null)
                {
                    //check if the column name has already been added
                    if (colnames.Contains(prop.NameKey))
                    {
                        continue;
                    }
                    try
                    {
                        r[prop.NameKey] = prop.Value;
                        colnames.Add(prop.NameKey);

                    }
                    catch
                    {
                    }
                }

            }

            try
            {
                OleDbCommandBuilder mybuilder = new OleDbCommandBuilder(daUsers);

                dsUsers.Tables[0].Rows.Add(r);
                DataSet dsNew = dsUsers.GetChanges(DataRowState.Added);
                daUsers.TableMappings.Add(TableName, TableName);
                // add the object
                daUsers.UpdateCommand = mybuilder.GetUpdateCommand();
                daUsers.Update(dsNew, TableName);
                // Insert the record even in the database
                dsUsers.AcceptChanges();
                // Align in-memory data with the data source ones
                colnames = null;
                result = true;

            }
            catch (Exception ex)
            {
                dsUsers.RejectChanges();
                // Reject DataSet changes
                Logger.WriteErrorToEventLog(ex);
            }

            return result;

        }

        public bool Insert<T>(string TableName, DataRow row, string constr)
        {
            var result = false;
            // Define a connection object
            OleDbConnection dbConn = new OleDbConnection(constr);


            OleDbDataAdapter daUsers = new OleDbDataAdapter("SELECT * FROM " + TableName + " WHERE ROWNUM  = 0", dbConn);
            // Create a data adapter to retrieve records from db
            DataSet dsUsers = new DataSet(TableName);
            // Create a data adapter to retrieve records from db

            daUsers.Fill(dsUsers);
            DataTableMapping dtmUsers = new DataTableMapping(TableName, TableName);

            DataRow r = dsUsers.Tables[0].NewRow();
            var props = getPropertyInfo<T>(row, default(T));
            var colnames = new List<string>();

            foreach (PropertyInfo prop in props)
            {

                if (prop != null)
                {
                    //check if the column name has already been added
                    if (colnames.Contains(prop.Name))
                    {
                        continue;
                        //dont bother to add
                    }

                    try
                    {
                        var ty = prop.PropertyType;
                        if (ty.IsGenericType && (object.ReferenceEquals(ty.GetGenericTypeDefinition(), typeof(Nullable<>))))
                        {
                            ty = Nullable.GetUnderlyingType(ty);
                        }
                        object value = ((IConvertible)row[prop.Name]).ToType(ty, null);

                        r[prop.Name] = value;

                        // Define the table containing the mapped columns
                        DataColumnMapping col = new DataColumnMapping(prop.Name, prop.Name);
                        dtmUsers.ColumnMappings.Add(col);
                        colnames.Add(prop.Name);

                    }
                    catch
                    {
                    }
                }

            }

            daUsers.TableMappings.Add(dtmUsers);
            // Activate the mapping mechanism

            try
            {
                OleDbCommandBuilder mybuilder = new OleDbCommandBuilder(daUsers);

                dsUsers.Tables[0].Rows.Add(r);
                DataSet dsNew = dsUsers.GetChanges(DataRowState.Added);
                // add the object
                daUsers.UpdateCommand = mybuilder.GetUpdateCommand();
                daUsers.Update(dsNew, TableName);
                // Insert the record even in the database
                dsUsers.AcceptChanges();
                // Align in-memory data with the data source ones
                colnames = null;
                result = true;
            }
            catch (Exception ex)
            {
                dsUsers.RejectChanges();// Reject DataSet changes
                Console.WriteLine(ex.Message);
                Logger.WriteErrorToEventLog(ex);

            }

            return result;

        }

        public bool Insert<T>(string TableName, T obj, string constr)
        {
            var result = false;
            // Define a connection object
            OleDbConnection dbConn = new OleDbConnection(constr);
            var tbl = getTableStructure(TableName, constr);

            OleDbDataAdapter daUsers = new OleDbDataAdapter("SELECT * FROM " + TableName + " WHERE ROWNUM  = 0", dbConn);
            // Create a data adapter to retrieve records from db
            DataSet dsUsers = new DataSet(TableName);
            // Create a data adapter to retrieve records from db

            daUsers.Fill(dsUsers);
            DataTableMapping dtmUsers = new DataTableMapping(TableName, TableName);


            DataRow r = dsUsers.Tables[0].NewRow();
            var props = getObjectInfo<T>(tbl, obj);

            var colnames = new List<string>();

            foreach (PropInfo prop in props)
            {
                if (prop != null)
                {
                    if (colnames.Contains(prop.NameKey))
                    {
                        continue;
                        //dont bother to add
                    }
                    try
                    {
                        r[prop.NameKey] = prop.Value;
                        // Define the table containing the mapped columns
                        DataColumnMapping col = new DataColumnMapping(prop.NameKey, prop.NameKey);
                        dtmUsers.ColumnMappings.Add(col);
                        colnames.Add(prop.NameKey);

                    }
                    catch
                    {
                    }
                }

            }

            daUsers.TableMappings.Add(dtmUsers);// Activate the mapping mechanism            

            try
            {
                OleDbCommandBuilder mybuilder = new OleDbCommandBuilder(daUsers);

                dsUsers.Tables[0].Rows.Add(r);
                DataSet dsNew = dsUsers.GetChanges(DataRowState.Added);
                daUsers.UpdateCommand = mybuilder.GetUpdateCommand();
                daUsers.Update(dsNew, TableName);
                // Insert the record even in the database
                dsUsers.AcceptChanges();
                // Align in-memory data with the data source ones

                colnames = null;
                result = true;
                // Align in-memory data with the data source ones
                result = true;
            }
            catch (Exception ex)
            {
                dsUsers.RejectChanges();// Reject DataSet changes                
                Console.WriteLine(ex.Message);
                Logger.WriteErrorToEventLog(ex);
            }

            return result;

        }

        #endregion

        public List<PropertyInfo> doIT(DataTable Tbl, object ob)
        {

            var result = new List<PropertyInfo>();
            var props = ob.GetType().GetProperties();

            foreach (PropertyInfo p in props)
            {
                var name = p.Name;
                var chk = from DataColumn x in Tbl.Columns where x.ColumnName.ToLower() == name.ToLower() select x;

                var ty = p.PropertyType;
                var ob_child = p.GetValue(ob, null);
                // get the property and its values as an object

                if (chk.Count() > 0)
                {
                    Console.WriteLine("property name found, {0}", p.Name);
                    result.Add(p);
                }
                else
                {
                    result.AddRange(doIT(Tbl, ob_child));
                }
            }

            return result;

        }

        public class PropInfo
        {

            private string _NameKey;

            private object _Value;
            public string NameKey
            {
                get { return _NameKey; }
                set { _NameKey = value; }
            }

            public object Value
            {
                get { return _Value; }
                set { _Value = value; }
            }

        }

        #endregion
    }

}
