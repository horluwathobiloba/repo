using AutoMapper;
using System;
using Onyx.WorkFlowService.Domain.Entities;
using Onyx.WorkFlowService.Application.Common.Mappings;
using Onyx.WorkFlowService.Domain.Enums;
using Onyx.WorkFlowService.Domain.Common;

namespace Onyx.WorkFlowService.Application.RolePermissions.Queries.GetRolePermissions
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
