using System;
using System.DirectoryServices.ActiveDirectory;
using System.Text;

namespace ReventInject.Services.AD
{
    public static class DomainManager
    {
        static DomainManager()
        {
            Domain domain = null;
            DomainController domainController = null;
            try
            {
                domain = Domain.GetCurrentDomain();
                DomainName = domain.Name;
                domainController = domain.PdcRoleOwner;
                DomainControllerName = domainController.Name.Split('.')[0];
                ComputerName = Environment.MachineName;

                //if (domain.Name != "fbncapitalltd.com")
                //{
                //    ComputerName = "FBNC-LP-DEL-141";
                //    DomainName = "fbncapitalltd.com";
                //    DomainControllerName = "FBNC-DCSVR-L01";
                //}
            }
            finally
            {
                if (domain != null)
                    domain.Dispose();
                if (domainController != null)
                    domainController.Dispose();
            }
        }

        public static string DomainControllerName { get; private set; }

        public static string ComputerName { get; private set; }

        public static string DomainName { get; private set; }

        public static string DomainPath
        {
            get
            {
                bool bFirst = true;
                StringBuilder sbReturn = new StringBuilder(200);
                string[] strlstDc = DomainName.Split('.');
                foreach (string strDc in strlstDc)
                {
                    if (bFirst)
                    {
                        sbReturn.Append("dc=");
                        bFirst = false;
                    }
                    else
                        sbReturn.Append(",dc=");

                    sbReturn.Append(strDc);
                }
                return sbReturn.ToString();
            }
        }

        public static string RootPath
        {
            get
            {
                return string.Format("LDAP://{0}/{1}", DomainName, DomainPath);
            }
        }
    }
}
