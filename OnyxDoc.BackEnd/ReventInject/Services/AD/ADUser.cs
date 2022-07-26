using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ReventInject.Services.AD
{

    [Serializable]
    [DataContract]
    public class ADUser
    {
        public ADUser()
        {

        }

        public ADUser(DirectoryEntry de)
        {

            try
            {
                this.displayname = de.Properties["displayName"].Value.ToString();
                this.firstname = de.Properties["givenName"].Value != null ? de.Properties["givenName"].Value.ToString() : null;
                this.username = de.Properties["samaccountname"].Value != null ? de.Properties["samaccountname"].Value.ToString() : null;
                this.lastname = de.Properties["sn"].Value != null ? de.Properties["sn"].Value.ToString() : null;
                this.email = de.Properties["mail"].Value != null ? de.Properties["mail"].Value.ToString() : null;
                this.department = de.Properties["department"].Value != null ? de.Properties["department"].Value.ToString() : null;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [DataMember]
        public bool isAuthenticated { get; set; }

        [DataMember]
        public string username { get; set; }

        [DataMember]
        public string firstname { get; set; }

        [DataMember]
        public string lastname { get; set; }

        [DataMember]
        public string displayname { get; set; }

        [DataMember]
        public string department { get; set; }

        [DataMember]
        public string email { get; set; }
    }



    [Serializable]
    [DataContract]
    public class ADGroup
    {
        public ADGroup()
        {

        }

        public ADGroup(string groupname, string short_name)
        {
            try
            {
                this.name = groupname;
                this.shortname = short_name;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string shortname { get; set; }

    }
}
