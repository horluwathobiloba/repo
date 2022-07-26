using AutoMapper;
using System;
using Onyx.AuthService.Domain.Entities;
using Onyx.AuthService.Application.Common.Mappings;
using Onyx.AuthService.Domain.Enums;
using Onyx.AuthService.Domain.Common;

namespace Onyx.AuthService.Application.RolePermissions.Queries.GetRolePermissions
{
    public class RolePermissionDto : IMapFrom<RolePermission>
    {
        public int RoleId { get; set; }
        public int OrganizationId { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public string Permission { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<RolePermission, RolePermissionDto>();
            profile.CreateMap<RolePermissionDto, RolePermission>();
        }
    }
}
