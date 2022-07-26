
using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;

namespace ReventInject.Services.AD
{

    public partial class ADServices
    {
        private string admin_uname;
        private string admin_pword;
        private DirectoryEntry de;
        private string ldapPath;
        //"qsbADUser";
        //"qu3st@#23%";


        //private IADServices sender;


        /// <summary>
        /// Initialises the client's active directory configuration
        /// </summary>
        /// <param name="smtpConfig"></param>
        public ADServices(string domain)
        {
            IADConfig _config = new ADFactory(domain);

            if (_config != null)
            {
                ADConfig config = _config.LoadADConfig();

                if (config != null)
                {
                    this.admin_uname = config.LDAPUsername;
                    this.admin_pword = config.LDAPPassword;
                    this.ldapPath = config.LDAPPath;
                    de = new DirectoryEntry(ldapPath, admin_uname, admin_pword, AuthenticationTypes.Secure);
                }
            }
            else
            {
                throw new Exception("Client's active directory configuration was not supplied.");
            }
        }


        /// <summary>
        /// Initialises the client's active directory configuration
        /// </summary>
        /// <param name="smtpConfig"></param>
        public ADServices(UserDomain domain)
        {
            IADConfig _config = new ADFactory(domain);

            if (_config != null)
            {
                ADConfig config = _config.LoadADConfig();

                if (config != null)
                {
                    this.admin_uname = config.LDAPUsername;
                    this.admin_pword = config.LDAPPassword;
                    this.ldapPath = config.LDAPPath;
                    de = new DirectoryEntry(ldapPath, admin_uname, admin_pword, AuthenticationTypes.Secure);
                }
            }
            else
            {
                throw new Exception("Client's active directory configuration was not supplied.");
            }
        }


        /// <summary>
        /// Initialize active directory services for a local application
        /// </summary>
        /// <param name="_admin_uname"></param>
        /// <param name="_admin_pword"></param>
        public ADServices(string _admin_uname, string _admin_pword)
        {
            admin_uname = _admin_uname;
            admin_pword = _admin_pword;
            ldapPath = DomainManager.RootPath;
            de = new DirectoryEntry(ldapPath, admin_uname, admin_pword, AuthenticationTypes.Secure);
        }

        /// <summary>
        /// Initialize active directory services for a particular active directory path
        /// </summary>
        /// <param name="_ldapPath"></param>
        /// <param name="_admin_uname"></param>
        /// <param name="_admin_pword"></param>
        public ADServices(string _ldapPath, string _admin_uname, string _admin_pword)
        {
            admin_uname = _admin_uname;
            admin_pword = _admin_pword;
            ldapPath = _ldapPath;
            de = new DirectoryEntry(ldapPath, admin_uname, admin_pword, AuthenticationTypes.Secure);
        }

        private DirectoryEntry GetDirectoryObject()
        {
            // string ldapPath = DomainManager.RootPath;
            DirectoryEntry de = new DirectoryEntry(ldapPath, admin_uname, admin_pword, AuthenticationTypes.Secure);
            return de;

        }

        private DirectoryEntry GetDirectoryObject(string UserName, string Password)
        {
            DirectoryEntry de = new DirectoryEntry(ldapPath, UserName, Password, AuthenticationTypes.Secure);
            return de;
        }

        private DirectoryEntry GetRootDirectoryEntry()
        {
            de = new DirectoryEntry(ldapPath, admin_uname, admin_pword);//, AuthenticationTypes.Secure
            return de;
        }


        private DirectoryEntry GetUser(string username)
        {
            DirectorySearcher deSearch = new DirectorySearcher();
            de = new DirectoryEntry(ldapPath, admin_uname, admin_pword);//, AuthenticationTypes.Secure
            deSearch.SearchRoot = de;


            deSearch.Filter = "(&((&(objectCategory=Person)(objectClass=User)))(samaccountname=" + username + "))";
            //deSearch.Filter = "(&(objectClass=user)(SAMAccountName=" + username + "))";

            deSearch.SearchScope = SearchScope.Subtree;
            SearchResult results = deSearch.FindOne();

            if ((results != null))
            {
                de = new DirectoryEntry(results.Path, admin_uname, admin_pword, AuthenticationTypes.Secure);
                return de;
            }
            else
            {
                return null;
            }

        }

        private DirectoryEntry GetUserByEmail(string email)
        {
            DirectorySearcher deSearch = new DirectorySearcher();
            deSearch.SearchRoot = de;

            deSearch.Filter = "(&((&(objectCategory=Person)(objectClass=User)))(mail=" + email + "))";
            // deSearch.Filter = "(&(objectClass=user)(SAMAccountName=" + UserName + "))";

            deSearch.SearchScope = SearchScope.Subtree;
            SearchResult results = deSearch.FindOne();

            if ((results != null))
            {
                de = new DirectoryEntry(results.Path, admin_uname, admin_pword, AuthenticationTypes.Secure);
                return de;
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// Authenticate users using their windows domain username and password
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool Login(string userName, string password)
        {
            bool authentic = false;
            try
            {
                DirectoryEntry entry = new DirectoryEntry(ldapPath, userName, password);
                object nativeObject = entry.NativeObject;
                authentic = true;
            }
            catch (DirectoryServicesCOMException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return authentic;
        }

        /// <summary>
        /// Authenticate users using their windows domain username and password and return the display name.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param> 
        /// <param name="display_name"></param>
        /// <returns></returns>
        public bool Authenticate(string userName, string password, ref string display_name)
        {
            bool authentic = false;
            try
            {
                DirectoryEntry entry = new DirectoryEntry(ldapPath, userName, password);
                object nativeObject = entry.NativeObject;

                // get the display name
                var de = GetUser(userName);

                if (de != null)
                {
                    display_name = de.Properties["displayName"].Value.ToString();
                }

                authentic = true;
            }
            catch (DirectoryServicesCOMException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return authentic;
        }

        /// <summary>
        /// Authenticate using domain username and password
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public ADUser Authenticate(string userName, string password)
        {
            ADUser x = null;

            if (string.IsNullOrEmpty(userName))
            {
                throw new Exception("username is required");
            }
            else if (string.IsNullOrEmpty(password))
            {
                throw new Exception("Password is required");

            }

            try
            {
                DirectoryEntry entry = new DirectoryEntry(ldapPath, userName, password);
                object nativeObject = entry.NativeObject;

                var de = GetUser(userName);

                if (de != null)
                {
                    x = new ADUser(de);
                    x.isAuthenticated = true;
                }
            }
            catch (DirectoryServicesCOMException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return x;
        }

        /// <summary>
        /// Authenticate using domain email and password
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param> 
        /// <param name="display_name"></param>
        /// <returns></returns>
        public ADUser AuthenticateByEmail(string email, string password, ref string display_name)
        {
            ADUser x = default(ADUser);

            if (string.IsNullOrEmpty(email))
            {
                throw new Exception("email is required");
            }
            else if (string.IsNullOrEmpty(password))
            {
                throw new Exception("Password is required");
            }

            try
            {
                // get the display name
                var de = GetUserByEmail(email);

                if (de != null)
                {
                    display_name = de.Properties["displayName"].Value.ToString();

                    string username = de.Properties["samaccountname"].Value != null ? de.Properties["samaccountname"].Value.ToString() : null;

                    DirectoryEntry entry = new DirectoryEntry(ldapPath, username, password);
                    object nativeObject = entry.NativeObject;

                    // get the display name
                    //de = GetUser(username);

                    if (de != null)
                    {
                        x = new ADUser(de);
                        x.isAuthenticated = true;
                    }
                }
            }
            catch (DirectoryServicesCOMException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return x;
        }

        public List<ADGroup> LoadGroups()
        {
            return null;
        }

        public ArrayList GetAllGroupNames()
        {
            DirectorySearcher deSearch = new DirectorySearcher();
            deSearch.SearchRoot = GetRootDirectoryEntry();
            deSearch.Filter = "(&(objectClass=group))";
            SearchResultCollection results = deSearch.FindAll();
            if (results.Count > 0)
            {
                ArrayList groupNames = new ArrayList();

                foreach (SearchResult group in results)
                {
                    var entry = new DirectoryEntry(group.Path, this.admin_uname, admin_pword);
                    string shortName = entry.Name.Substring(3, entry.Name.Length - 3);
                    groupNames.Add(shortName);
                }

                return groupNames;
            }
            else
            {
                return new ArrayList();
            }
        }

        public List<ADGroup> GetDomainGroups()
        {
            DirectorySearcher deSearch = new DirectorySearcher();
            deSearch.SearchRoot = GetRootDirectoryEntry();
            // Binding path. 
            List<ADGroup> result = new List<ADGroup>();



            // Get search object, specify filter and scope, 
            // perform search. 
            try
            {
                deSearch.Filter = "(&(objectClass=group))";
                deSearch.SearchScope = SearchScope.Subtree;
                SearchResultCollection objSearchResults = deSearch.FindAll();

                if (objSearchResults.Count != 0)
                {
                    foreach (SearchResult objResult in objSearchResults)
                    {
                        var objGroupEntry = objResult.GetDirectoryEntry();
                        string shortName = objGroupEntry.Name.Substring(3, objGroupEntry.Name.Length - 3);
                        var group = new ADGroup(objGroupEntry.Name, shortName);
                        result.Add(group);
                    }
                }
                else
                {
                    throw new Exception("No groups found");
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return result;
        }

        /// <summary>
        /// Find active directory user using the domain username
        /// </summary>
        /// <param name="usrname"></param>
        /// <returns></returns>
        public ADUser FindUser(string username)
        {
            ADUser x = default(ADUser);

            if (string.IsNullOrEmpty(username))
            {
                throw new Exception("username is required");
            }

            try
            {
                var de = GetUser(username);

                if (de != null)
                {
                    x = new ADUser(de);
                    x.isAuthenticated = false;
                }
            }
            catch (DirectoryServicesCOMException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return x;
        }

        /// <summary>
        /// Find active directory user using email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public ADUser FindUserByEmail(string email)
        {
            ADUser x = default(ADUser);

            if (string.IsNullOrEmpty(email))
            {
                throw new Exception("email is required");
            }

            try
            {
                var de = GetUserByEmail(email);

                if (de != null)
                {
                    x = new ADUser(de);
                    x.isAuthenticated = false;
                }
            }
            catch (DirectoryServicesCOMException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return x;
        }

        /// <summary>
        /// This method will attempt to log in a user based on the username and password to ensure that they have been set up 
        /// within the Active Directory. This is the basic UserName, Password check.
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        public bool IsUserValid(string UserName)
        {
            try
            {
                //if the object can be created then return true
                DirectoryEntry dirUser = GetUser(UserName);
                dirUser.Close();
                return true;
            }
            catch (Exception ex)
            {
                //otherwise return false
                return false;
            }
        }

        /// <summary>
        /// This method will attempt to log in a user based on the username and password to ensure that they have been set up 
        /// within the Active Directory. This is the basic UserName, Password check.
        /// </summary>
        /// <param name="email"></param>
        public bool IsUserValidByEmail(string email)
        {
            try
            {
                //if the object can be created then return true
                DirectoryEntry dirUser = GetUserByEmail(email);
                dirUser.Close();
                return true;
            }
            catch (Exception ex)
            {
                //otherwise throw the exception
                throw ex;
            }
        }

        //"LDAP://dc2.gtalimited.com/dc=gtalimited,dc=com";

        /// <summary>
        /// Gets the display name as entered in the AD using username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public string getFullName(string username)
        {
            dynamic result = string.Empty;
            de = GetUser(username);
            if (de != null)
            {
                result = de.Properties["displayName"].Value;
            }
            return result;
        }


        /// <summary>
        /// Gets the display name as entered in the AD using email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public string getFullNameByEmail(string email)
        {
            dynamic result = string.Empty;
            de = GetUserByEmail(email);
            if (de != null)
            {
                result = de.Properties["displayName"].Value;
            }
            return result;
        }


        /// <summary>
        /// Change password
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        private bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            string path = DomainManager.RootPath; //"LDAP://CN=" + userName + ",CN=Users,DC=demo,DC=domain,DC=com";
            try
            {
                DirectoryEntry directoryEntry = new DirectoryEntry(path, admin_uname, admin_pword);
                directoryEntry.Invoke("ChangePassword", new object[] { oldPassword, newPassword });
                Console.WriteLine("success");
                return true;
            }
            catch (Exception ex)  //TODO: catch a specific exception ! :)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }


}
