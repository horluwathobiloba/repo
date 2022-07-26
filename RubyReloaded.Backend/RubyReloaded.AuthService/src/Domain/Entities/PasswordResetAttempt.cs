using RubyReloaded.AuthService.Domain.Common;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Domain.Entities
{
    public class PasswordResetAttempt : AuditableEntity
    {
        public PasswordResetStatus PasswordResetStatus { get; set; }
        public string PasswordResetStatusDesc { get; set; }
        public string Email { get; set; }
    }
}
