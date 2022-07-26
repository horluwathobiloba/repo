using Onyx.AuthService.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.AuthService.Domain.Entities
{
    public class UserRefreshToken:AuditableEntity
    {
        public string Email { get; set; }
        public string RefreshTokens { get; set; }
        public DateTime RefreshTokenExpires { get; set; }
    }
}
