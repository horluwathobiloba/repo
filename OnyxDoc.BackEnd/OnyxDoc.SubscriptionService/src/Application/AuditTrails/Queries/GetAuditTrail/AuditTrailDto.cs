using AutoMapper;
using OnyxDoc.SubscriptionService.Application.Common.Mappings;
using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Domain.Enums;

namespace OnyxDoc.SubscriptionService.Application.AuditTrails.Queries
{
        public class AuditTrailDto : IMapFrom<AuditTrail>
    {
        public string UserId { get; set; }
        public AuditAction AuditAction { get; set; }
        public string AuditActionDesc { get; set; }
        public string ControllerName { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
        public string AffectedColumns { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string MicroserviceName { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.AuditTrail, AuditTrailDto>();
            profile.CreateMap<AuditTrailDto, Domain.Entities.AuditTrail>();
        }
    }
}
