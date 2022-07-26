using AutoMapper;
using OnyxDoc.DocumentService.Application.Common.Mappings;
using OnyxDoc.DocumentService.Domain.Entities;
using OnyxDoc.DocumentService.Domain.Enums;

namespace OnyxDoc.DocumentService.Application.AuditTrails.Queries
{
        public class AuditTrailDto : IMapFrom<Domain.Entities.AuditTrail>
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
            profile.CreateMap<AuditTrail, AuditTrailDto>();
            profile.CreateMap<AuditTrailDto, AuditTrail>();
        }
    }
}
