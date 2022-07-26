
using AutoMapper;
using RubyReloaded.AuthService.Application.Common.Mappings;
using RubyReloaded.AuthService.Domain.Common;
using RubyReloaded.AuthService.Domain.Entities;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;

namespace RubyReloaded.AuthService.Application.RolePermissions.Queries.GetRolePermissions
{
    public class RolePermissionListDto :  IMapFrom<CooperativeRolePermission>
{
        public int RoleId { get; set; }
        public int OrganizationId { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public string Permission { get; set; }
        public Status Status { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CooperativeRolePermission, RolePermissionListDto> ();
            profile.CreateMap<RolePermissionListDto, CooperativeRolePermission>();
        }
    }
}
