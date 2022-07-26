using AutoMapper;
using System;
using RubyReloaded.AuthService.Domain.Entities;
using RubyReloaded.AuthService.Application.Common.Mappings;
using RubyReloaded.AuthService.Domain.Enums;
using RubyReloaded.AuthService.Domain.Common;

namespace RubyReloaded.AuthService.Application.Roles.Queries.GetRoles
{
    public class RoleDto : IMapFrom<CooperativeRole>
    {
        public int Id { get; set; }
        public Cooperative Cooperative { get; set; }
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
            profile.CreateMap<CooperativeRole, RoleDto>();
            profile.CreateMap<RoleDto, CooperativeRole>();
        }
    }
}
