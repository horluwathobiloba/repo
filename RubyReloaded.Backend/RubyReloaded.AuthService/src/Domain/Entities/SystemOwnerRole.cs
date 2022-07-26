using RubyReloaded.AuthService.Domain.Common;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
namespace RubyReloaded.AuthService.Domain.Entities
{
    public class SystemOwnerRole:AuditableEntity
    {
        public int SystemOwnerId { get; set; }
        public SystemOwner SystemOwner { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public string AccessLevelDesc { get; set; } 
        //public List<SystemOwnerRolePermission> RolePermissions { get; set; }
    }
}
