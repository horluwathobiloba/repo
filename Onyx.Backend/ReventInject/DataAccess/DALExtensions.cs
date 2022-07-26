﻿
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Diagnostics;
using System.ComponentModel;
using System.Reflection;
using System.Data.SqlClient;

namespace ReventInject.DataAccess
{

    public class DALExtensions
    {
        private static string constr;
        public DALExtensions(string constring)
        {
            constr = constring;
        }

        public static DataSet ToDataSet<T>(IList<T> list, string TableName)
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

        private static bool IsNullableType(Type myType)
        {
            return (myType.IsGenericType) && (object.ReferenceEquals(myType.GetGenericTypeDefinition(), typeof(Nullable<>)));
        }

        public static DataTable ToDataTable<T>(IList<T> list, string TableName)
        {

            Type elementType = typeof(T);
            DataSet ds = new DataSet();
            DataTable tb = new DataTable();
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

        public static DataSet ToDataSet2<T>(IList<T> list, string TableName)
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

            PropertyInfo[] props = elementType.GetProperties();
            var cls = elementType.GetMembers().Where(x => x.MemberType == MemberTypes.Property | x.MemberType == MemberTypes.NestedType | x.MemberType == MemberTypes.Field).ToList();

            //add a column to table for each public property on T 

            foreach (MemberInfo memInfo in cls)
            {

                if (memInfo.MemberType == MemberTypes.Property | memInfo.MemberType == MemberTypes.Field)
                {
                    var pname = memInfo.Name;
                    var chk = (from DataColumn x in dt.Columns where x.ColumnName.Equals( pname, StringComparison.OrdinalIgnoreCase) select x).ToList();

                    if (chk.Count > 0)
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

                        var chk = (from DataColumn x in dt.Columns where x.ColumnName.Equals( pname, StringComparison.OrdinalIgnoreCase) select x).ToList();

                        if (chk.Count > 0)
                        {
                            PropertyInfo c = props.Where(h => h.Name.Equals(pname, StringComparison.OrdinalIgnoreCase)).First();
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

        public static DataTable ToDataTable2<T>(List<T> list, string TableName)
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
                    var chk = (from DataColumn x in dt.Columns where x.ColumnName.Equals( pname, StringComparison.OrdinalIgnoreCase) select x).ToList();

                    if (chk.Count > 0)
                    {
                        PropertyInfo c = props.Where(h => h.Name.Equals(pname, StringComparison.OrdinalIgnoreCase)).First();
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


                foreach (PropertyInfo propInfo in memtypes)
                {
                    if (propInfo.MemberType == MemberTypes.Property | propInfo.MemberType == MemberTypes.Field)
                    {
                        var pname = propInfo.Name;

                        var chk = (from DataColumn x in dt.Columns where x.ColumnName.Equals( pname, StringComparison.OrdinalIgnoreCase) select x).ToList();

                        if (chk.Count > 0)
                        {
                            PropertyInfo c = props.Where(h => h.Name.Equals(pname, StringComparison.OrdinalIgnoreCase)).First();
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

        public class CollectionHelper
        {

            public static DataTable ConvertTo<T>(IList<T> list)
            {
                DataTable table = CreateTable<T>();
                Type entityType = typeof(T);
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

                foreach (T item in list)
                {
                    DataRow row = table.NewRow();

                    foreach (PropertyDescriptor prop in properties)
                    {
                        row[prop.Name] = prop.GetValue(item) == null || string.IsNullOrWhiteSpace(prop.GetValue(item).ToString()) ? DBNull.Value : (object)prop.GetValue(item);
                    }

                    table.Rows.Add(row);
                }

                return table;
            }

            public static IList<T> ConvertTo<T>(IList<DataRow> rows)
            {
                IList<T> list = null;

                if ((rows != null))
                {
                    list = new List<T>();

                    foreach (DataRow row in rows)
                    {
                        T item = CreateItem<T>(row);
                        list.Add(item);
                    }
                }

                return list;
            }

            public static IList<T> ConvertTo<T>(DataTable table)
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

                return ConvertTo<T>(rows);
            }

            public static bool IsNullableType(Type myType)
            {
                return (myType.IsGenericType) && (object.ReferenceEquals(myType.GetGenericTypeDefinition(), typeof(Nullable<>)));
            }

            public static T CreateItem<T>(DataRow row)
            {
                T obj = default(T);
                if ((row != null))
                {
                    obj = Activator.CreateInstance<T>();

                    foreach (DataColumn column in row.Table.Columns)
                    {
                       
                        try
                        {
                             PropertyInfo prop = (from z in obj.GetType().GetProperties()
                                            where z.Name.Equals(column.ColumnName, StringComparison.InvariantCultureIgnoreCase)
                                                select z).FirstOrDefault();

                            object value = row[column.ColumnName];

                            if (value == null)
                            {
                                continue;
                            }
                            else
                            {
                                if (Nullable.GetUnderlyingType(prop.PropertyType) != null)
                                {
                                    if (Nullable.GetUnderlyingType(prop.PropertyType).Name.Equals("decimal", StringComparison.InvariantCultureIgnoreCase))
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
                               
                            }
                        }
                        catch (Exception ex)
                        {
                            // You can log something here

                        }
                    }
                }

                return obj;
            }

            public static DataTable CreateTable<T>()
            {
                Type entityType = typeof(T);
                DataTable table = new DataTable(entityType.Name);
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

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

                return table;
            }

        }
    }

}
