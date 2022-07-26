using RubyReloaded.AuthService.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Domain.Entities
{
    public class SystemOwner:AuditableEntity
    {
        public string ContactEmail { get; set; }
        public string RCNumber { get; set; }
        public string ContactPhoneNumber { get; set; }
    }
}
