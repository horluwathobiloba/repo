using OnyxDoc.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.AuthService.Application.Common.Models
{
    public class RolePermissionsVm
    {
        public Status Status { get; set; }

        public string Permission { get; set; }
    }
}
