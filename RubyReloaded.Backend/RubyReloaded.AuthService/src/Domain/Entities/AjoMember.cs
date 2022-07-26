using RubyReloaded.AuthService.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Domain.Entities
{
    public class AjoMember:AuditableEntity
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public int AjoId { get; set; }
        public Ajo Ajo { get; set; }
        public int RoleId { get; set; }
    }
}
