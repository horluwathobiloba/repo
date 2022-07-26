using System.Text.Json.Serialization;

namespace RubyReloaded.WalletService.Application.Common.Models
{
    public class AuthToken
    {
        [JsonIgnore]
        public string AccessToken { get; set; }
    }
}