using OnyxDoc.AuthService.Domain.Common;
using OnyxDoc.AuthService.Domain.Enums;

namespace OnyxDoc.AuthService.Domain.Entities
{
    public class DefaultRolesConfiguration : AuditableEntity
    {
        public Subscriber Subscriber { get; set; }
        public RoleAccessLevel RoleAccessLevel { get; set; }
        public string RoleAccessLevelDesc { get; set; }
        public DefaultRoles RoleName { get; set; }
        public SubscriberType SubscriberType { get; set; }
    }
}
