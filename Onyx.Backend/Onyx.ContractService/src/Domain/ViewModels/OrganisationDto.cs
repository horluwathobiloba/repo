using Newtonsoft.Json;
using Onyx.ContractService.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.ContractService.Domain.Entities
{
    public class OrganisationDto : AuthEntity
    {

        [JsonProperty("rcnumber")]
        public string RCNumber { get; set; }
        [JsonProperty("contactemail")]
        public string ContactEmail { get; set; }
        [JsonProperty("contactphonenumber")]
        public string ContactPhoneNumber { get; set; }
        [JsonProperty("address")]
        public string Address { get; set; }
        [JsonProperty("country")]
        public string Country { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("logofilelocation")]
        public string LogoFileLocation { get; set; }
        [JsonProperty("themecolor")]
        public string ThemeColor { get; set; }
    }
}
