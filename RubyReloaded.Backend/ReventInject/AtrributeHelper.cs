using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace ReventInject.AttributeHelper
{

    [AttributeUsage(AttributeTargets.Property)]
    public class RequiredField : System.Attribute
    {
        public static void PerformCheck(object objectToCheck)
        {
            var properties = new List<PropertyInfo>();
            var type = objectToCheck.GetType();


            properties = type.GetProperties().Where(prop => prop.GetCustomAttributes(typeof(RequiredField), true).Any()).ToList();

            //verbose code
            //foreach (var prop in type.GetProperties())
            //    if (prop.GetCustomAttributes(typeof(Required), true).Any())
            //        properties.Add(prop);

            foreach (var prop in properties)
            {
                object val = prop.GetValue(objectToCheck, null);
                //Validation logic here
                if (val == null)
                {
                    throw new ArgumentNullException(string.Format("{0} is required and cannot be null.", prop.Name));
                }
                //else
                //continue;
            }
        }

        public static void PerformNullCheck(Type objectToCheck)
        {
            var properties = new List<PropertyInfo>();
            var type = objectToCheck;
            foreach (var prop in type.GetProperties())
                if (prop.GetCustomAttributes(typeof(RequiredField), true).Any())
                    properties.Add(prop);
            foreach (var prop in properties)
            {
                object value = prop.GetValue(objectToCheck, null);
                //Validation logic here
                if (value == null)
                    throw new ArgumentNullException(string.Format("{0} is required and cannot be null.", prop.Name));
            }
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class NonNegative : System.Attribute
    {
        public static void PerformNonNegativeCheck(object objectToCheck)
        {
            var properties = new List<PropertyInfo>();
            var type = objectToCheck.GetType();



            properties = type.GetProperties().Where(prop => prop.GetCustomAttributes(typeof(NonNegative), true).Any()).ToList();

            //verbose code
            //foreach (var prop in type.GetProperties())
            //    if (prop.GetCustomAttributes(typeof(NonNegative), true).Any())
            //        properties.Add(prop);


            foreach (var prop in properties)
            {
                object val = prop.GetValue(objectToCheck, null);
                //Validation logic here
                if (val == null)
                {
                    throw new ArgumentNullException(string.Format("{0} is required and cannot be null!", prop.Name));
                }
                else if (!val.IsNumeric())
                {
                    throw new InvalidDataException(string.Format("{0} must be a number!", prop.Name));
                }
                //else
                //do nothing

                string ty = prop.PropertyType.Name.ToLower();

                switch (ty)
                {
                    case "decimal":
                        if ((decimal)(val) < 0)
                        {
                            throw new ArgumentOutOfRangeException(string.Format("{0} cannot be less than 0!", prop.Name));
                        }
                        break;

                    case "float":
                        if ((float)(val) < 0)
                        {
                            throw new ArgumentOutOfRangeException(string.Format("{0} cannot be less than 0!", prop.Name));
                        }
                        break;

                    case "double":
                        if ((double)(val) < 0)
                        {
                            throw new ArgumentOutOfRangeException(string.Format("{0} cannot be less than 0!", prop.Name));

                        }
                        break;

                    case "int32":
                    case "int":
                    case "int16":
                        if ((int)(val) < 0)
                        {
                            throw new ArgumentOutOfRangeException(string.Format("{0} cannot be less than 0!", prop.Name));
                        }
                        break;
                    case "long":
                    case "int64":
                        if ((long)(val) < 0)
                        {
                            throw new ArgumentOutOfRangeException(string.Format("{0} cannot be less than 0!", prop.Name));
                        }
                        break;

                    default:
                        if (decimal.Parse((string)(val)) < 0)
                        {
                            throw new ArgumentOutOfRangeException(string.Format("{0} cannot be less than 0!", prop.Name));
                        }
                        break;
                }

            }
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class DateField : System.Attribute
    {
        public static void PerformCheck(object objectToCheck)
        {
            var properties = new List<PropertyInfo>();
            var type = objectToCheck.GetType();


            properties = type.GetProperties().Where(prop => prop.GetCustomAttributes(typeof(DateField), true).Any()).ToList();

            //verbose code
            //foreach (var prop in type.GetProperties())
            //    if (prop.GetCustomAttributes(typeof(Required), true).Any())
            //        properties.Add(prop);

            foreach (var prop in properties)
            {
                object val = prop.GetValue(objectToCheck, null);
                //Validation logic here
                if (val == null)
                {
                    throw new ArgumentNullException(string.Format("{0} is required and cannot be null.", prop.Name));
                }
                else if( !val.IsDate()  )
                {
                    throw new ArgumentNullException(string.Format("{0} requires a valid date value.", prop.Name));
                }
                //continue;
            }
        }

        public static void PerformNullCheck(Type objectToCheck)
        {
            var properties = new List<PropertyInfo>();
            var type = objectToCheck;
            foreach (var prop in type.GetProperties())
                if (prop.GetCustomAttributes(typeof(DateField), true).Any())
                    properties.Add(prop);
            foreach (var prop in properties)
            {
                object value = prop.GetValue(objectToCheck, null);
                //Validation logic here
                if (value == null)
                    throw new ArgumentNullException(string.Format("{0} is required and cannot be null.", prop.Name));
            }
        }
    }

    public class XmlSynonymDeserializer : XmlSerializer
    {
        public class SynonymsAttribute : Attribute
        {
            public readonly ISet<string> Names;

            public SynonymsAttribute(params string[] names)
            {
                this.Names = new HashSet<string>(names);
            }

            public static MemberInfo GetMember(object obj, string name)
            {
                Type type = obj.GetType();

                var result = type.GetProperty(name);
                if (result != null)
                    return result;

                foreach (MemberInfo member in type.GetProperties().Cast<MemberInfo>().Union(type.GetFields()))
                    foreach (var attr in member.GetCustomAttributes(typeof(SynonymsAttribute), true))
                        if (attr is SynonymsAttribute && ((SynonymsAttribute)attr).Names.Contains(name))
                            return member;

                return null;
            }
        }

        public XmlSynonymDeserializer(Type type)
            : base(type)
        {
            this.UnknownElement += this.SynonymHandler;
        }

        public XmlSynonymDeserializer(Type type, XmlRootAttribute root)
            : base(type, root)
        {
            this.UnknownElement += this.SynonymHandler;
        }

        protected void SynonymHandler(object sender, XmlElementEventArgs e)
        {
            var member = SynonymsAttribute.GetMember(e.ObjectBeingDeserialized, e.Element.Name);
            Type memberType;

            if (member != null && member is FieldInfo)
                memberType = ((FieldInfo)member).FieldType;
            else if (member != null && member is PropertyInfo)
                memberType = ((PropertyInfo)member).PropertyType;
            else
                return;

            if (member != null)
            {
                object value;
                XmlSynonymDeserializer serializer = new XmlSynonymDeserializer(memberType, new XmlRootAttribute(e.Element.Name));
                using (System.IO.StringReader reader = new System.IO.StringReader(e.Element.OuterXml))
                    value = serializer.Deserialize(reader);

                if (member is FieldInfo)
                    ((FieldInfo)member).SetValue(e.ObjectBeingDeserialized, value);
                else if (member is PropertyInfo)
                    ((PropertyInfo)member).SetValue(e.ObjectBeingDeserialized, value);
            }
        }
    }

    public static partial class InjectGlobal
    {
        public static void PerformCheck(object objectToCheck)
        {
            var properties = new List<PropertyInfo>();
            var type = objectToCheck.GetType();

            properties = type.GetProperties().Where(prop => prop.GetCustomAttributes(typeof(RequiredField), true).Any()).ToList();

            //verbose code
            //foreach (var prop in type.GetProperties())
            //    if (prop.GetCustomAttributes(typeof(Required), true).Any())
            //        properties.Add(prop);

            foreach (var prop in properties)
            {
                object value = prop.GetValue(objectToCheck, null);
                //Validation logic here
                if (value == null)
                {
                    throw new ArgumentNullException(string.Format("Value of property {0} is required!", prop.Name));
                }
            }
        }

        public static void PerformNullCheck(this object objectToCheck)
        {
            var properties = new List<PropertyInfo>();
            var type = objectToCheck.GetType();

            properties = type.GetProperties().Where(prop => prop.GetCustomAttributes(typeof(RequiredField), true).Any()).ToList();

            //verbose code
            //foreach (var prop in type.GetProperties())
            //    if (prop.GetCustomAttributes(typeof(Required), true).Any())
            //        properties.Add(prop);

            foreach (var prop in properties)
            {
                object value = prop.GetValue(objectToCheck, null);
                //Validation logic here
                if (value == null)
                    throw new ArgumentNullException(string.Format("Value of property {0} is required!", prop.Name));
            }
        }

        public static void PerformNullCheck(this Type objectToCheck)
        {
            var properties = new List<PropertyInfo>();
            var type = objectToCheck.GetType();

            properties = type.GetProperties().Where(prop => prop.GetCustomAttributes(typeof(RequiredField), true).Any()).ToList();

            //verbose code
            //foreach (var prop in type.GetProperties())
            //    if (prop.GetCustomAttributes(typeof(Required), true).Any())
            //        properties.Add(prop);

            foreach (var prop in properties)
            {
                object value = prop.GetValue(objectToCheck, null);
                //Validation logic here
                if (value == null)
                {
                    throw new ArgumentNullException(string.Format("Value of property {0} is required!", prop.Name));
                }
            }
        }

        public static void PerformNonNegativeCheck(object objectToCheck)
        {
            var properties = new List<PropertyInfo>();
            var type = objectToCheck.GetType();

            properties = type.GetProperties().Where(prop => prop.GetCustomAttributes(typeof(NonNegative), true).Any()).ToList();

            //verbose code
            //foreach (var prop in type.GetProperties())
            //    if (prop.GetCustomAttributes(typeof(Required), true).Any())
            //        properties.Add(prop);

            foreach (var prop in properties)
            {
                object val = prop.GetValue(objectToCheck, null);

                //Validation logic here
                if (val == null)
                {
                    throw new ArgumentNullException(string.Format("{0} is required and cannot be null!", prop.Name));
                }
                else if (!val.IsNumeric())
                {
                    throw new InvalidDataException(string.Format("{0} must be a number!", prop.Name));
                }
                //else
                //do nothing

                string ty = prop.PropertyType.Name.ToLower();

                switch (ty)
                {
                    case "decimal":
                        if ((decimal)(val) < 0)
                        {
                            throw new ArgumentOutOfRangeException(string.Format("{0} cannot be less than 0!", prop.Name));
                        }
                        break;

                    case "float":
                        if ((float)(val) < 0)
                        {
                            throw new ArgumentOutOfRangeException(string.Format("{0} cannot be less than 0!", prop.Name));
                        }
                        break;

                    case "double":
                        if ((double)(val) < 0)
                        {
                            throw new ArgumentOutOfRangeException(string.Format("{0} cannot be less than 0!", prop.Name));

                        }
                        break;

                    case "int32":
                    case "int":
                    case "int16":
                        if ((int)(val) < 0)
                        {
                            throw new ArgumentOutOfRangeException(string.Format("{0} cannot be less than 0!", prop.Name));
                        }
                        break;
                    case "long":
                    case "int64":
                        if ((long)(val) < 0)
                        {
                            throw new ArgumentOutOfRangeException(string.Format("{0} cannot be less than 0!", prop.Name));
                        }
                        break;

                    default:
                        if (decimal.Parse((string)(val)) < 0)
                        {
                            throw new ArgumentOutOfRangeException(string.Format("{0} cannot be less than 0!", prop.Name));
                        }
                        break;
                }
            }
        }


        public static Byte[] ToByteArray(this Stream stream)
        {
            Int32 length = stream.Length > Int32.MaxValue ? Int32.MaxValue : Convert.ToInt32(stream.Length);
            Byte[] buffer = new Byte[length];
            stream.Read(buffer, 0, length);
            return buffer;
        }

    }

}

