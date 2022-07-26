using System.Text.Json.Serialization;

namespace OnyxDoc.SubscriptionService.Application.Common.Models
{
    public class AuthToken
    {
        [JsonIgnore]
        public string AccessToken { get; set; }
    }
}