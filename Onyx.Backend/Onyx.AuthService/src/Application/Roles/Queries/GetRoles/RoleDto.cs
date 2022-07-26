using AutoMapper;
using System;
using Onyx.AuthService.Domain.Entities;
using Onyx.AuthService.Application.Common.Mappings;
using Onyx.AuthService.Domain.Enums;
using Onyx.AuthService.Domain.Common;

namespace Onyx.AuthService.Application.Roles.Queries.GetRoles
{
    public class RoleDto : IMapFrom<Role>
    {
        public int Id { get; set; }
        public Organization Organization { get; set; }
        public int OrganizationId { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public string AccessLevelDesc { get; set; }
        public WorkflowUserCategory WorkflowUserCategory { get; set; }
        public string WorkflowUserCategoryDesc { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Role, RoleDto>();
            profile.CreateMap<RoleDto, Role>();
        }
    }
}
