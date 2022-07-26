using System.Text.Json.Serialization;

namespace Onyx.ContractService.Application.Common.Models
{
    public class AuthToken
    {
        [JsonIgnore]
        public string AccessToken { get; set; }
    }
}