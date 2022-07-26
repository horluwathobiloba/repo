using Onyx.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.AuthService.Domain.ViewModels
{
    public class RolePermissionsVm
    {
        public Status Status { get; set; }

        public string Permission { get; set; }

    }
}
