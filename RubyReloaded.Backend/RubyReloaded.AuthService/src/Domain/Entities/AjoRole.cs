using RubyReloaded.AuthService.Domain.Common;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Domain.Entities
{
    public class AjoRole:AuditableEntity
    {
        public int AjoId { get; set; }
        public Ajo Ajo { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public string AccessLevelDesc { get; set; }
        //public List<AjoRolePermission> RolePermissions { get; set; }
    }
}
