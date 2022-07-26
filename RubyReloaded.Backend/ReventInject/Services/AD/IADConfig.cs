using System.ComponentModel.DataAnnotations;

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
