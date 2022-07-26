using System.Text.Json.Serialization;

namespace OnyxDoc.DocumentService.Application.Common.Models
{
    public class AuthToken
    {
        [JsonIgnore]
        public string AccessToken { get; set; }
    }
}