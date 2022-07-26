using System;
using System.Text;
using System.Collections.Generic;

namespace ReventInject.Utilities
{
    public class EnumBinder
    {

        public static List<object> GetEnumNames(string enumStringType)
        {
            List<object> result = new List<object>();
            Type enumType = Type.GetType(enumStringType);

            foreach (object enumValue in Enum.GetValues(enumType))
            {
                string enumName = Enum.GetName(enumType, enumValue);
                dynamic data = new
                {
                    Name = SpaceWords(enumName),
                    Value = Convert.ToInt32(enumValue)
                };
                result.Add(data);
            }
            return result;
        }

        private static string SpaceWords(string words)
        {
            StringBuilder result = new StringBuilder();
            foreach (char letter in words.ToCharArray())
            {
                if (char.IsUpper(letter))
                {
                    if (words.IndexOf(letter) != 0)
                    {
                        result.Append(' ');
                    }
                }
                result.Append(letter);
            }
            return result.Replace('_', ' ').ToString();
        }

        public static List<EnumDictionary> LoadEnum(string enumStringType)
        {
            List<EnumDictionary> result = new List<EnumDictionary>();
            Type enumType = Type.GetType(enumStringType);

            foreach (object enumValue in Enum.GetValues(enumType))
            {
                string enumName = Enum.GetName(enumType, enumValue);
                EnumDictionary data = new EnumDictionary
                {
                    EnumText = enumName,
                    EnumID = Convert.ToInt32(enumValue)
                };
                result.Add(data);
            }
            return result;
        }
    }

    public class EnumDictionary
    {

        private int m_EnumID;

        private string m_EnumText;
        public int EnumID
        {
            get { return m_EnumID; }
            set { m_EnumID = value; }
        }

        public string EnumText
        {
            get { return m_EnumText; }
            set { m_EnumText = value; }
        }

        public class EnumUtil<T>
        {

            public static T NumToEnum(int number)
            {
                return (T)Enum.ToObject(typeof(T), number);
            }

        }

    }

}
