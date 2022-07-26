namespace OnyxDoc.AuthService.Application.Common.Models
{
    public class AuthToken
    {
        public long ExpiresIn { get; set; }

        public string AccessToken { get; set; }

        public string UserToken { get; set; }
    }
}