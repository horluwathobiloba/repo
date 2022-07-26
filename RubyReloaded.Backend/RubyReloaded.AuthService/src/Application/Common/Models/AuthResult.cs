using System.Collections.Generic;

namespace RubyReloaded.AuthService.Application.Common.Models
{ 
    public class AuthResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public AuthToken Token { get; set; }
        public bool OnBoardingStatus { get; set; }
        public bool Verified { get; set; }
        public List<int> Cooperatives { get; set; }
        public List<int> Ajos { get; set; }
    }
}