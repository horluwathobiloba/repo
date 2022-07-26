using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.Data.SqlTypes;
using System.Runtime.Serialization;
using System.Linq;
using System.Threading.Tasks;
using ReventInject.Entities;
using ReventInject.Utilities.Enums;

namespace ReventInject.Utilities
{

    public class AuditSerializer
    {
        /// <summary>
        /// Serializes the object.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public byte[] ToByte(object obj)
        {
            byte[] serializedObject = null;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.TypeFormat = System.Runtime.Serialization.Formatters.FormatterTypeStyle.TypesAlways;
            bf.AssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Full;
            bf.Serialize(ms, obj);
            serializedObject = ms.ToArray();
            ms.Close();


            bf.Serialize(ms, obj);
            return serializedObject;

        }

        public string ObjectToString(object obj)
        {
            string strObj = string.Empty;

            try
            {
                strObj = System.Convert.ToBase64String(ToByte(obj));

            }
            catch (Exception ex)
            {
                Logger.WriteErrorToEventLog(ex);
            }
            return strObj;
        }

        public byte[] ToBinary(string val)
        {
            byte[] b = null;
            try
            {
                b = System.Convert.FromBase64String(val);

            }
            catch (Exception ex)
            {
                Logger.WriteErrorToEventLog(ex);
            }
            return b;
        }

        /// <summary>
        /// Serializes the object.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public byte[] SerializeObject(object obj)
        {
            byte[] serializedObject = null;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            //NetDataContractSerializer bf = new NetDataContractSerializer();
            BinaryFormatter bf = new BinaryFormatter();

            // bf.TypeFormat = System.Runtime.Serialization.Formatters.FormatterTypeStyle.TypesAlways
            bf.AssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Full;
            bf.Serialize(ms, obj);
            serializedObject = ms.ToArray();
            ms.Close();
            return serializedObject;
        }

        /// <summary>
        /// Deserializes the object.
        /// </summary>
        /// <param name="serializedObject">The serialized object.</param>
        /// <returns></returns>
        public object DeSerializeObject(byte[] serializedObject)
        {
            object deSerializedObject = null;
            if ((serializedObject != null))
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream(serializedObject);
                try
                {
                    // deSerializedObject = new NetDataContractSerializer().Deserialize(ms);
                    deSerializedObject = new BinaryFormatter().Deserialize(ms);
                }
                catch
                {
                }
                finally
                {
                    ms.Close();
                }
            }
            return deSerializedObject;
        }


        public List<TrailItem> Deserialize(byte[] databefore, byte[] dataafter, string dataType, ActionType action)
        {
            List<TrailItem> ofItems = new List<TrailItem>();
            object dafter = null;
            if (dataafter == null)
            {
                dafter = new object();
            }
            else
            {
                dafter = (DeSerializeObject(dataafter));
            }
            object dbefore = null;
            if (databefore == null)
            {
                dbefore = new object();
            }
            else
            {
                dbefore = (DeSerializeObject(databefore));
            }

            object dtemp = dbefore;
            //create
            if ((dataafter != null) && databefore == null)
            {
                dtemp = dafter;
            }
            //delete
            if ((databefore != null) && dataafter == null)
            {
                dtemp = dbefore;
            }

            if (dtemp == null)
            {
                return ofItems;
            }
            foreach (PropertyInfo prop in dtemp.GetType().GetProperties())
            {
                Type ptype = prop.PropertyType;
                object ater = null;
                object bfore = null;
                //if ((ptype.IsPrimitive || ptype.Equals(typeof(String)) || ptype.Equals(typeof(DateTime)) || ptype.IsEnum) && prop.CanWrite)
                // if (prop.GetCustomAttributes(typeof(IsLoggedAttribute), false).Length > 0)
                string before = "";
                string after = "";
                string name = string.Empty;
                //((IsLoggedAttribute)prop.GetCustomAttributes(typeof(IsLoggedAttribute), false)[0]).LoggedName;
                if (name == string.Empty)
                {
                    name = prop.Name;
                }
                if (!(action == ActionType.Add))
                {
                    bfore = prop.GetValue(dbefore, null);
                    if (ptype.Equals(typeof(DateTime)))
                    {
                        DateTime noDate = Convert.ToDateTime(SqlDateTime.Null);
                        if ((Convert.ToDateTime(bfore)).Equals(noDate))
                        {
                            before = ("None");
                        }
                        else
                        {
                            before = ((Convert.ToDateTime(bfore)).ToString("dd-MMM-yyyy hh:mm tt"));
                        }
                    }
                    else if (ptype.Equals(typeof(decimal)))
                    {
                        before = (Convert.ToDecimal(bfore)).ToString("N");
                    }
                    else if (ptype.IsClass && (!ptype.Equals(typeof(string))))
                    {
                        try
                        {
                            before = ptype.GetProperty("Name").GetValue(bfore, null).ToString();
                        }
                        catch
                        {
                            if (bfore == null)
                            {
                                before = "";
                            }
                            else
                            {
                                before = bfore.ToString();
                            }
                        }
                    }
                    else
                    {
                        if (bfore == null)
                        {
                            before = "";
                        }
                        else
                        {
                            before = bfore.ToString();
                        }
                    }

                }

                if (!(action == ActionType.Delete))
                {
                    ater = prop.GetValue(dafter, null);
                    if (ptype.Equals(typeof(DateTime)))
                    {
                        DateTime noDate = Convert.ToDateTime(SqlDateTime.Null);
                        if ((Convert.ToDateTime(ater)).Equals(noDate))
                        {
                            after = ("None");
                        }
                        else
                        {
                            after = ((Convert.ToDateTime(ater)).ToString("dd-MMM-yyyy hh:mm tt"));
                        }
                    }
                    else if (ptype.Equals(typeof(decimal)))
                    {
                        after = (Convert.ToDecimal(ater)).ToString("N2");
                    }
                    else if (ptype.IsClass && (!ptype.Equals(typeof(string))))
                    {
                        try
                        {
                            after = ptype.GetProperty("Name").GetValue(ater, null).ToString();
                            //new CoreSystem().Retrieve(ptype.GetProperty("ID").GetValue(ater, null), ptype), null).ToString();
                        }
                        catch
                        {
                            if (ater == null)
                            {
                                after = "";
                            }
                            else
                            {
                                after = ater.ToString();
                            }
                        }
                    }
                    else
                    {
                        if (ater == null)
                        {
                            after = "";
                        }
                        else
                        {
                            after = ater.ToString();
                        }
                    }
                }
                //Don't show "Deleted" or "ID" status.
                //if (prop.Name != "ID" && prop.Name != "Deleted")
                ofItems.Add(new TrailItem(name, before, after));
            }

            return ofItems;

        }

        public byte[] SerializeData(object objectBefore, object objectAfter)
        {
            return SerializeObject(ConvertToList(objectBefore, objectAfter));
        }

        public List<TrailItem> DeSerializeObject(byte[] dataBefore, byte[] dataAfter)
        {
            return ConvertToList(DeSerializeObject(dataBefore), DeSerializeObject(dataAfter));
        }

        public List<TrailItem> ConvertToList(object objectBefore, object objectAfter)
        {
            List<TrailItem> trailItems = new List<TrailItem>();

            if (objectBefore == null && objectAfter == null)
            {
                return trailItems;
            }

            if ((objectBefore != null) && (objectAfter != null))
            {
                if ((!object.ReferenceEquals(objectBefore.GetType(), objectAfter.GetType())))
                {
                    throw new InvalidOperationException("The two objects must be of the same type");
                }
            }

            //Use one of them to invoke reflection.
            object dataTypeCheck = null;
            if ((objectAfter != null))
            {
                dataTypeCheck = objectAfter;
            }
            else
            {
                dataTypeCheck = objectBefore;
            }
            dynamic count = 0;

            try
            {
                //Iterate thru all the properties

                foreach (PropertyInfo propertyInfo in dataTypeCheck.GetType().GetProperties())
                {
                    Type propertyType = propertyInfo.PropertyType;
                    object propertyAfter = null;
                    object propertyBefore = null;

                    string strBefore = "";
                    string strAfter = "";
                    string name = "";
                    if ((!string.IsNullOrEmpty(propertyInfo.Name)))
                    {
                        name = propertyInfo.Name;
                    }

                    if ((objectBefore != null))
                    {
                        try
                        {
                            propertyBefore = propertyInfo.GetValue(Convert.ChangeType(objectBefore, dataTypeCheck.GetType()), null);

                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    if ((objectAfter != null))
                    {
                        try
                        {
                            propertyAfter = propertyInfo.GetValue(Convert.ChangeType(objectAfter, dataTypeCheck.GetType()), null);

                        }
                        catch (Exception ex)
                        {
                        }

                    }

                    if (propertyType.Equals(typeof(DateTime)))
                    {
                        DateTime noDate = Convert.ToDateTime(SqlDateTime.Null);
                        if ((propertyBefore != null))
                        {
                            if ((Convert.ToDateTime(propertyBefore)).Equals(noDate))
                            {
                                strBefore = ("None");
                            }
                            else
                            {
                                strBefore = ((Convert.ToDateTime(propertyBefore)).ToString("dd-MMM-yyyy hh:mm tt"));
                            }
                        }
                        if ((propertyAfter != null))
                        {
                            if ((Convert.ToDateTime(propertyAfter)).Equals(noDate))
                            {
                                strAfter = ("None");
                            }
                            else
                            {
                                strAfter = ((Convert.ToDateTime(propertyAfter)).ToString("dd-MMM-yyyy hh:mm tt"));
                            }
                        }
                    }
                    else if (propertyType.Equals(typeof(decimal)))
                    {
                        if ((propertyBefore != null))
                        {
                            strBefore = (Convert.ToDecimal(propertyBefore)).ToString("N");
                        }
                        if ((propertyAfter != null))
                        {
                            strAfter = (Convert.ToDecimal(propertyAfter)).ToString("N");
                        }


                    }
                    else if (propertyType.IsClass && (!propertyType.Equals(typeof(string))))
                    {
                        //Remember to also check the properties of the subclass for Loggable attribute.
                        //Dim innerClassProperties As List(Of TrailItem) = GetInnerClassProperties(propertyBefore, propertyAfter)
                        //If Not innerClassProperties Is Nothing AndAlso innerClassProperties.Count > 0 Then
                        //    trailItems.AddRange(innerClassProperties)
                        //End If
                        continue;
                        //'End If
                    }
                    else
                    {
                        if (propertyBefore == null)
                        {
                            strBefore = "";
                        }
                        else
                        {
                            strBefore = propertyBefore.ToString();
                        }
                        if (propertyAfter == null)
                        {
                            strAfter = "";
                        }
                        else
                        {
                            strAfter = propertyAfter.ToString();
                        }
                    }

                    trailItems.Add(new TrailItem(name, strBefore, strAfter));

                    count = count + 1;
                    //End If
                }

            }
            catch (Exception ex)
            {
                dynamic c = count;
            }

            return trailItems;

        }

        private List<TrailItem> GetInnerClassProperties(object objectBefore, object objectAfter)
        {
            return ConvertToList(objectBefore, objectAfter);
        }

    }

}
