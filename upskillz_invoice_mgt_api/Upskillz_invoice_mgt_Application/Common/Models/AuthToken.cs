using System.Text.Json.Serialization;

namespace Upskillz_invoice_mgt_Application.Common.Models
{
    public class AuthToken
    {
        [JsonIgnore]
        public string AccessToken { get; set; }
    }
}
