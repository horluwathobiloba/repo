using System.Configuration;

namespace ReventInject
{

    public enum SysMode
    {
        Dev = 1,
        SIT = 2,
        UAT = 3,
        Live = 4
    }

    public interface ISysConfig
    {

        bool TestMode { get; }
        string AppName { get; }
        string AppFolder { get; }

    }

    public class SysConfig
    {

        public static bool TestMode
        {
            get
            {
                string xmode = ConfigurationManager.AppSettings["xmode"];
                switch (xmode)
                {
                    case "1":
                        return true;
                    case "2":
                        return true;
                    case "3":
                        return true;
                    case "4":
                        return false;
                    default:
                        return true;
                }
            }

        }

        public static int AppMode
        {
            get
            {
                string xmode = ConfigurationManager.AppSettings["xmode"];
                switch (xmode)
                {
                    case "1":
                        return (int)SysMode.Dev;
                    case "2":
                        return (int)SysMode.SIT;
                    case "3":
                        return (int)SysMode.UAT;
                    case "4":
                        return (int)SysMode.Live;
                    default:
                        return (int)SysMode.Dev;
                }
            }
        }

        public static string AppName
        {
            get
            {
                return ConfigurationManager.AppSettings["AppName"];
            }
        }


        #region Client LDAP Settings
        public static string Client_LDAPUsername
        {
            get
            {
                return ConfigurationManager.AppSettings["Client_LDAPUsername"];
            }
        }

        public static string Client_LDAPPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["Client_LDAPPassword"];
            }
        }

        public static string Client_LDAPPath
        {
            get
            {
                return ConfigurationManager.AppSettings["Client_LDAPPath"];
            }
        }
        #endregion


        #region FBNMB LDAP Settings
        public static string FBNMB_LDAPUsername
        {
            get
            {
                return ConfigurationManager.AppSettings["FBNMB_LDAPUsername"];
            }
        }

        public static string FBNMB_LDAPPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["FBNMB_LDAPPassword"];
            }
        }

        public static string FBNMB_LDAPPath
        {
            get
            {
                return ConfigurationManager.AppSettings["FBNMB_LDAPPath"];
            }
        }

        #endregion

        #region FBNC LDAP Settings
        public static string FBNC_LDAPUsername
        {
            get
            {
                return ConfigurationManager.AppSettings["FBNC_LDAPUsername"];
            }
        }

        public static string FBNC_LDAPPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["FBNC_LDAPPassword"];
            }
        }

        public static string FBNC_LDAPPath
        {
            get
            {
                return ConfigurationManager.AppSettings["FBNC_LDAPPath"];
            }
        }
        #endregion

        public static string AppFolder
        {
            get
            {
                string folda = ConfigurationManager.AppSettings["AppFolder"];
                if (string.IsNullOrEmpty(folda))
                {
                    if (TestMode)
                    {
                        if (folda.Trim().EndsWith("\\"))
                        {
                            folda = folda + "Test\\";
                        }
                        else
                        {
                            folda = folda + "\\Test\\";
                        }
                    }
                }
                return folda;
            }
        }

        public static string AppKey
        {
            get
            {
                return ConfigurationManager.AppSettings["AppKey"];
            }
        }    


    }


}
