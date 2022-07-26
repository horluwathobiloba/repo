using System.Text.Json.Serialization;

namespace OnyxDoc.FormBuilderService.Application.Common.Models
{
    public class AuthToken
    {
        [JsonIgnore]
        public string AccessToken { get; set; }
    }
}