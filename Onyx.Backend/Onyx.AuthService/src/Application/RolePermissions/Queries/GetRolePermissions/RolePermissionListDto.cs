
using AutoMapper;
using Onyx.AuthService.Application.Common.Mappings;
using Onyx.AuthService.Domain.Common;
using Onyx.AuthService.Domain.Entities;
using Onyx.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Onyx.AuthService.Application.RolePermissions.Queries.GetRolePermissions
{
    public class RolePermissionListDto :  IMapFrom<RolePermission>
{
        public int RoleId { get; set; }
        public int OrganizationId { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public string Permission { get; set; }
        public Status Status { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<RolePermission, RolePermissionListDto> ();
            profile.CreateMap<RolePermissionListDto, RolePermission>();
        }
    }
}
