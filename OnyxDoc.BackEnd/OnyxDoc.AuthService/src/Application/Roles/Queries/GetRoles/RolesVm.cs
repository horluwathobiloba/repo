using OnyxDoc.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.AuthService.Application.Roles.Queries.GetRoles
{
    public class RolesVm
    {
        public string Name { get; set; }
        public RoleAccessLevel RoleAccessLevel { get; set; }
    }
}
