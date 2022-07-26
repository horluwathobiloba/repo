using RubyReloaded.AuthService.Domain.Enums;
using System.Collections.Generic;

namespace RubyReloaded.AuthService.Application.RolePermissions.Queries.GetRolePermissions
{
    public class RolePermissionsVm
    {
        public Status Status { get; set; }

        public string Permission { get; set; }
    }
}
