using AutoMapper;
using System;
using RubyReloaded.AuthService.Domain.Entities;
using RubyReloaded.AuthService.Application.Common.Mappings;
using RubyReloaded.AuthService.Domain.Enums;
using RubyReloaded.AuthService.Domain.Common;

namespace RubyReloaded.AuthService.Application.RolePermissions.Queries.GetRolePermissions
{
    public class RolePermissionDto : IMapFrom<CooperativeRolePermission>
    {
        public int RoleId { get; set; }
        public int OrganizationId { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public string Permission { get; set; }
        public Status Status { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CooperativeRolePermission, RolePermissionDto>();
            profile.CreateMap<RolePermissionDto, CooperativeRolePermission>();
        }
    }
}
