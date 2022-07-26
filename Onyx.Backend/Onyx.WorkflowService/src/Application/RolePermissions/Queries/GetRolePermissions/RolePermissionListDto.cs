
using AutoMapper;
using Onyx.WorkFlowService.Application.Common.Mappings;
using Onyx.WorkFlowService.Domain.Common;
using Onyx.WorkFlowService.Domain.Entities;
using Onyx.WorkFlowService.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Onyx.WorkFlowService.Application.RolePermissions.Queries.GetRolePermissions
{
    public class RolePermissionListDto :  IMapFrom<RolePermission>
{
        public int RoleId { get; set; }
        public int OrganizationId { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public string Permission { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<RolePermission, RolePermissionListDto> ();
            profile.CreateMap<RolePermissionListDto, RolePermission>();
        }
    }
}
