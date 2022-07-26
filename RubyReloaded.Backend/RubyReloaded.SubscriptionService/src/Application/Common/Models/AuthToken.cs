using System.Text.Json.Serialization;

namespace RubyReloaded.SubscriptionService.Application.Common.Models
{
    public class AuthToken
    {
        [JsonIgnore]
        public string AccessToken { get; set; }
    }
}