using RubyReloaded.AuthService.Domain.Common;
using RubyReloaded.AuthService.Domain.Enums;

namespace RubyReloaded.AuthService.Domain.Entities
{
    public class SystemOwnerRolePermission: AuditableEntity
    {
        public int SystemOwnerId { get; set; }
        public int RoleId { get; set; }
        // public int OrganizationId { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public string Permission { get; set; }
        public string Category { get; set; }
    }
}