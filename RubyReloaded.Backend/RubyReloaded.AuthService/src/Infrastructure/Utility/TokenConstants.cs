namespace RubyReloaded.AuthService.Infrastructure.Auth
{ 
    public class TokenConstants
    {
        public const string Issuer = "RubyReloadedAuthService";
        public const string Audience = "RubyReloadedAuthService";
        public const int ExpiryInMinutes = 1440;
        public const string key = "RubyReloadedAuthService";
    }
}