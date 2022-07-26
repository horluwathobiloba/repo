using AutoMapper;
using System;
using OnyxDoc.AuthService.Domain.Entities;
using OnyxDoc.AuthService.Application.Common.Mappings;
using OnyxDoc.AuthService.Domain.Enums;
using OnyxDoc.AuthService.Domain.Common;

namespace OnyxDoc.AuthService.Application.Roles.Queries.GetRoles
{
    public class RoleDto : IMapFrom<Role>
    {
        public int Id { get; set; }
        public Subscriber Subscriber { get; set; }
        public int SubscriberId { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }
        public RoleAccessLevel RoleAccessLevel { get; set; }
        public string AccessLevelDesc { get; set; }
        public string WorkflowUserCategoryDesc { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Role, RoleDto>();
            profile.CreateMap<RoleDto, Role>();
        }
    }
}
