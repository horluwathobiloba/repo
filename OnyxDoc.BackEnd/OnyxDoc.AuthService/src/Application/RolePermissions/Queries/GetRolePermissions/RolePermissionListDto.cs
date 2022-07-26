
using AutoMapper;
using OnyxDoc.AuthService.Application.Common.Mappings;
using OnyxDoc.AuthService.Domain.Common;
using OnyxDoc.AuthService.Domain.Entities;
using OnyxDoc.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;

namespace OnyxDoc.AuthService.Application.RolePermissions.Queries.GetRolePermissions
{
    public class RolePermissionListDto :  IMapFrom<RolePermission>
{
        public int RoleId { get; set; }
        public int SubscriberId { get; set; }
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
