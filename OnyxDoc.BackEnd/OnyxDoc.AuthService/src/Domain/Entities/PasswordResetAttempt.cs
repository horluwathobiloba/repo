using OnyxDoc.AuthService.Domain.Common;
using OnyxDoc.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.AuthService.Domain.Entities
{
    public class PasswordResetAttempt : AuditableEntity
    {
        public PasswordResetStatus PasswordResetStatus { get; set; }
        public string PasswordResetStatusDesc { get; set; }
        public string Email { get; set; }
    }
}
