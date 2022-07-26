namespace Onyx.AuthService.Application.Common.Models
{ 
    public class AuthResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public AuthToken Token { get; set; }
    }
}