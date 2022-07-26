using AutoMapper;
using System;
using OnyxDoc.AuthService.Domain.Entities;
using OnyxDoc.AuthService.Application.Common.Mappings;
using OnyxDoc.AuthService.Domain.Enums;
using OnyxDoc.AuthService.Domain.Common;
using System.Collections.Generic;

namespace OnyxDoc.AuthService.Application.RolePermissions.Queries.GetRolePermissions
{
    public class RolePermissionDto : IMapFrom<RolePermission>
    {
        public int RoleId { get; set; }
        public int SubscriberId { get; set; }
        public RoleAccessLevel AccessLevel { get; set; }
        public string Permission { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<RolePermission, RolePermissionDto>();
            profile.CreateMap<RolePermissionDto, RolePermission>();
        }

        
    }
}
