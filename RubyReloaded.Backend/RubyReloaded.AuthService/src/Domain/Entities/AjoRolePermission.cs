using RubyReloaded.AuthService.Domain.Common;
using RubyReloaded.AuthService.Domain.Enums;

namespace RubyReloaded.AuthService.Domain.Entities
{
    public class AjoRolePermission:AuditableEntity
    {
        public int AjoId { get; set; }
        public int RoleId { get; set; }
        public string Category { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public string Permission { get; set; }
    }
}