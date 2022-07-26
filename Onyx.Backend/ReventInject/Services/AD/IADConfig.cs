using ReventInject;
using ReventInject.Utilities; 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ReventInject.Services.AD
{

    public interface IADConfig
    {
        /// <summary>
        /// This method will be used to load the client's active directory configurations for authentication
        /// </summary>
        /// <param name="port"></param>
        /// <param name="mailfrom"></param>
        /// <param name="usesAuth"></param>
        /// <param name="smtpUsername"></param>
        /// <param name="password"></param>
        /// <param name="enableSsl"></param>
        /// <returns>The active directory config</returns>   
       ADConfig LoadADConfig();
    }

    public class ADConfig
    {
        [Required]
        public string LDAPUsername { set; get; }

        [Required]
        public string LDAPPassword { set; get; }

        [Required]
        public string LDAPPath { set; get; } 
    }

}
