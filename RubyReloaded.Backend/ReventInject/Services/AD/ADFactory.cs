using System;

namespace ReventInject.Services.AD
{

    /// <summary>
    /// AD service factory. supports multiple domain based on the number of domains subscribed to or the client's domain
    /// </summary>
    public class ADFactory : IADConfig
    {

        private UserDomain domain;
        private string domain_name;

        /// <summary>
        /// Initialise the active directory service for the specified domain
        /// </summary>
        /// <param name="_domain"></param>
        public ADFactory(UserDomain _domain)
        {
            domain = _domain;  
        }

        /// <summary>
        /// Initialise the active directory service for the specified domain
        /// </summary>
        /// <param name="_domain_name"></param>
        public ADFactory(string _domain_name)
        {
            domain_name = _domain_name;

            try
            {               

                if (string.IsNullOrEmpty(domain_name))
                {
                    throw new Exception("Domain name canno tbe null!");
                }

                if (domain_name.ToLower().Contains("fbnquestmb.com")
                    || domain_name.ToLower().Contains("fbnmerchantbank.com"))
                {
                    domain = UserDomain.FBNQuestMB;
                }
                else if (domain_name.ToLower().Contains("fbnquest.com") 
                    || domain_name.ToLower().Contains("fbncapital.com")
                    || domain_name.ToLower().Contains("fbnquestam.com")
                    || domain_name.ToLower().Contains("fbnquestsec.com")
                    || domain_name.ToLower().Contains("fbnquestrustees.com"))
                {
                    domain = UserDomain.FBNQuest;
                }
                else
                {
                    domain = UserDomain.Local;
                }
                 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method is used to load the bank's AD server based on the subscriber's domain
        /// </summary>
        /// <returns></returns>
        public ADConfig LoadADConfig()
        {
            ADConfig config = new ADConfig();

            try
            {              
                switch (domain)
                {
                    case UserDomain.FBNQuestMB:
                        {

                            config.LDAPUsername = SysConfig.FBNMB_LDAPUsername;
                            config.LDAPPassword = SysConfig.FBNMB_LDAPPassword;
                            config.LDAPPath = SysConfig.FBNMB_LDAPPath;

                            break;
                        }

                    case UserDomain.FBNQuest:
                        {
                            config.LDAPUsername = SysConfig.FBNC_LDAPUsername;
                            config.LDAPPassword = SysConfig.FBNC_LDAPPassword;
                            config.LDAPPath = SysConfig.FBNC_LDAPPath;

                            break;
                        }
                    case UserDomain.Local:
                        {

                            config.LDAPUsername = SysConfig.Client_LDAPUsername;
                            config.LDAPPassword = SysConfig.Client_LDAPPassword;
                            config.LDAPPath = SysConfig.Client_LDAPPath;

                            break;
                        }
                    default:
                        {
                            throw new Exception("Invalid user domain specified! This domain is not supported for active directory login.");
                        }
                }


                return config;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ADConfig ADConfig { get; set; }

    }
}
