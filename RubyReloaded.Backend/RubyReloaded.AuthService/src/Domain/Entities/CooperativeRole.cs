using RubyReloaded.AuthService.Domain.Common;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Domain.Entities
{
    public class CooperativeRole:AuditableEntity
    {
        public int CooperativeId { get; set; }
        public Cooperative Cooperative { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public string AccessLevelDesc { get; set; }
       // public List<CooperativeRolePermission> RolePermissions { get; set; }
    }
}
