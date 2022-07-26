using RubyReloaded.AuthService.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Domain.Entities
{
    public class SystemOwnerUsers:AuditableEntity
    {
        public int SystemOwnerId { get; set; }
        public int RoleId { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
    }
}
