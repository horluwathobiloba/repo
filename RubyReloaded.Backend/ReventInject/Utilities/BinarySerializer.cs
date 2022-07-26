using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.Data.SqlTypes;
using ReventInject.Entities;
using ReventInject.Utilities.Enums;

namespace ReventInject.Utilities
{

    public class BinarySerializer
	{

		/// <summary>
		/// Serializes the object.
		/// </summary>
		/// <param name="obj">The obj.</param>
		/// <returns></returns>
		public static byte[] SerializeObject(object obj)
		{
			byte[] serializedObject = null;
			System.IO.MemoryStream ms = new System.IO.MemoryStream();
			BinaryFormatter bf = new BinaryFormatter();
			bf.TypeFormat = System.Runtime.Serialization.Formatters.FormatterTypeStyle.TypesAlways;
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
		public static object DeSerializeObject(byte[] serializedObject)
		{
			object deSerializedObject = null;
			if ((serializedObject != null)) {
				System.IO.MemoryStream ms = new System.IO.MemoryStream(serializedObject);
				try {
					deSerializedObject = new AuditSerializer().DeSerializeObject(serializedObject);
					//New BinaryFormatter().Deserialize(ms)
				} catch {
				} finally {
					ms.Close();
				}
			}
			return deSerializedObject;
		}


		public static List<TrailItem> Deserialize(byte[] databefore, byte[] dataafter, string dataType, AuditAction action)
		{
			List<TrailItem> ofItems = new List<TrailItem>();
			object dafter = null;
			if (dataafter == null) {
				dafter = new object();
			} else {
				dafter = (DeSerializeObject(dataafter));
			}
			object dbefore = null;
			if (databefore == null) {
				dbefore = new object();
			} else {
				dbefore = (DeSerializeObject(databefore));
			}

			object when = dbefore;
			//create
			if ((dataafter != null) && databefore == null) {
				when = dafter;
			}
			//delete
			if ((databefore != null) && dataafter == null) {
				when = dbefore;
			}

			if (when == null) {
				return ofItems;
			}
			foreach (PropertyInfo prop in when.GetType().GetProperties()) {
				Type ptype = prop.PropertyType;
				object ater = null;
				object bfore = null;
				//if ((ptype.IsPrimitive or ptype.Equals(typeof(String)) or ptype.Equals(typeof(DateTime)) or ptype.IsEnum) && prop.CanWrite)
				// if (prop.GetCustomAttributes(typeof(IsLoggedAttribute), false).Length > 0)
				string before = "";
				string after = "";
				string name = string.Empty;
				if (name == string.Empty) {
					name = prop.Name;
				}
				if (!(action == AuditAction.Save)) {
					bfore = prop.GetValue(dbefore, null);
					if (ptype.Equals(typeof(DateTime))) {
						DateTime noDate = Convert.ToDateTime(SqlDateTime.Null);
						if ((Convert.ToDateTime(bfore)).Equals(noDate)) {
							before = ("None");
						} else {
							before = ((Convert.ToDateTime(bfore)).ToString("dd-MMM-yyyy hh:mm tt"));
						}
					} else if (ptype.Equals(typeof(decimal))) {
						before = (Convert.ToDecimal(bfore)).ToString("N2");
					} else if (ptype.IsClass && (!ptype.Equals(typeof(string)))) {
						try {
							before = ptype.GetProperty("Name").GetValue(bfore, null).ToString();
						} catch {
							if (bfore == null) {
								before = "";
							} else {
								before = bfore.ToString();
							}
						}
					} else {
						if (bfore == null) {
							before = "";
						} else {
							before = bfore.ToString();
						}
					}

				}

				if (!(action == AuditAction.Delete)) {
					ater = prop.GetValue(dafter, null);
					if (ptype.Equals(typeof(DateTime))) {
						DateTime noDate = Convert.ToDateTime(SqlDateTime.Null);
						if ((Convert.ToDateTime(ater)).Equals(noDate)) {
							after = ("None");
						} else {
							after = ((Convert.ToDateTime(ater)).ToString("dd-MMM-yyyy hh:mm tt"));
						}
					} else if (ptype.Equals(typeof(decimal))) {
						after = (Convert.ToDecimal(ater)).ToString("N");
					} else if (ptype.IsClass && (!ptype.Equals(typeof(string)))) {
						try {
							after = ptype.GetProperty("Name").GetValue(ater, null).ToString();
							//new CoreSystem().Retrieve(ptype.GetProperty("ID").GetValue(ater, null), ptype), null).ToString();
						} catch {
							if (ater == null) {
								after = "";
							} else {
								after = ater.ToString();
							}
						}
					} else {
						if (ater == null) {
							after = "";
						} else {
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

		public static byte[] SerializeData(object objectBefore, object objectAfter)
		{
			return BinarySerializer.SerializeObject(ConvertToList(objectBefore, objectAfter));
		}

		public static List<TrailItem> DeSerializeObject(byte[] dataBefore, byte[] dataAfter)
		{
			return ConvertToList(DeSerializeObject(dataBefore), DeSerializeObject(dataAfter));
		}

		public static List<TrailItem> ConvertToList(object objectBefore, object objectAfter)
		{
			List<TrailItem> trailItems = new List<TrailItem>();

			if (objectBefore == null && objectAfter == null) {
				return trailItems;
			}

			if ((objectBefore != null) && (objectAfter != null)) {
				if ((!object.ReferenceEquals(objectBefore.GetType(), objectAfter.GetType()))) {
					throw new InvalidOperationException("The two objects must be of the same type");
				}
			}

			//Use one of them to invoke reflection.
			object dataTypeCheck = null;
			if ((objectAfter != null)) {
				dataTypeCheck = objectAfter;
			} else {
				dataTypeCheck = objectBefore;
			}


			//Iterate thru all the properties

			foreach (PropertyInfo propertyInfo in dataTypeCheck.GetType().GetProperties()) {
				Type propertyType = propertyInfo.PropertyType;
				object propertyAfter = null;
				object propertyBefore = null;

				string strBefore = "";
				string strAfter = "";
				string name = "";
				if ((!string.IsNullOrEmpty(propertyInfo.Name))) {
					name = propertyInfo.Name;
				}

				if ((objectBefore != null)) {
					propertyBefore = propertyInfo.GetValue(Convert.ChangeType(objectBefore, dataTypeCheck.GetType()), null);
				}
				if ((objectAfter != null)) {
					propertyAfter = propertyInfo.GetValue(Convert.ChangeType(objectAfter, dataTypeCheck.GetType()), null);
				}

				if (propertyType.Equals(typeof(DateTime))) {
					DateTime noDate = Convert.ToDateTime(SqlDateTime.Null);
					if ((propertyBefore != null)) {
						if ((Convert.ToDateTime(propertyBefore)).Equals(noDate)) {
							strBefore = ("None");
						} else {
							strBefore = ((Convert.ToDateTime(propertyBefore)).ToString("dd-MMM-yyyy hh:mm tt"));
						}
					}
					if ((propertyAfter != null)) {
						if ((Convert.ToDateTime(propertyAfter)).Equals(noDate)) {
							strAfter = ("None");
						} else {
							strAfter = ((Convert.ToDateTime(propertyAfter)).ToString("dd-MMM-yyyy hh:mm tt"));
						}
					}
				} else if (propertyType.Equals(typeof(decimal))) {
					if ((propertyBefore != null)) {
						strBefore = (Convert.ToDecimal(propertyBefore)).ToString("N");
					}
					if ((propertyAfter != null)) {
						strAfter = (Convert.ToDecimal(propertyAfter)).ToString("N");
					}


				} else if (propertyType.IsClass && (!propertyType.Equals(typeof(string)))) {
					//Remember to also check the properties of the subclass for Loggable attribute.
					List<TrailItem> innerClassProperties = GetInnerClassProperties(propertyBefore, propertyAfter);
					if ((innerClassProperties != null) && innerClassProperties.Count > 0) {
						trailItems.AddRange(innerClassProperties);
					}
					continue;
					//End If
				} else {
					if (propertyBefore == null) {
						strBefore = "";
					} else {
						strBefore = propertyBefore.ToString();
					}
					if (propertyAfter == null) {
						strAfter = "";
					} else {
						strAfter = propertyAfter.ToString();
					}
				}

				trailItems.Add(new TrailItem(name, strBefore, strAfter));
				//End If
			}
			return trailItems;

		}

		private static List<TrailItem> GetInnerClassProperties(object objectBefore, object objectAfter)
		{
			return ConvertToList(objectBefore, objectAfter);
		}

	}



}

