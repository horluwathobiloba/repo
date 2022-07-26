using AutoMapper;
using OnyxDoc.AuthService.Application.Common.Mappings;
using OnyxDoc.AuthService.Domain.Entities;
using OnyxDoc.AuthService.Domain.Enums;

namespace OnyxDoc.AuthService.Application.Clients.Queries
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
