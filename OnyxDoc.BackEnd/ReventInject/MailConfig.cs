using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReventInject
{
    internal static class MailConfig
    {
        public static string SmtpHost
        {
            get
            {
                return ConfigurationManager.AppSettings["SmtpHost"];
            }
        }

        public static string Proxy
        {
            get
            {
                return ConfigurationManager.AppSettings["Proxy"];
            }
        }

        public static long MaxRetry
        {
            get
            {
                try
                {
                    return long.Parse(ConfigurationManager.AppSettings["MaxRetry"]);
                }
                catch (Exception ex)
                {
                    return 5;
                }
            }
        }

        public static string SmtpPort
        {
            get
            {
                return ConfigurationManager.AppSettings["SmtpPort"];
            }
        }

        public static string MailFrom
        {
            get
            {
                return ConfigurationManager.AppSettings["MailFrom"];
            }
        }

        public static string SmtpUserName
        {
            get
            {
                return ConfigurationManager.AppSettings["SmtpUserName"];
            }
        }

        public static string SmtpPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["SmtpPassword"];
            }
        }

        public static bool UsesAuthentication
        {
            get
            {
                var v = ConfigurationManager.AppSettings["UsesAuthentication"];
                try
                {
                    return v == "1";

                }
                catch (Exception ex)
                {
                    return false;
                }

            }
        }

        public static string WebsiteURL
        {
            get
            {
                return ConfigurationManager.AppSettings["WebsiteURL"];
            }
        }


    }

}
