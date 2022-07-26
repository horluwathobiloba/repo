using OnyxDoc.AuthService.Domain.Common;
using OnyxDoc.AuthService.Domain.Enums;

using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.AuthService.Domain.Entities
{
    public class RolePermission : AuditableEntity
    {
        public int RoleId { get; set; }
        public RoleAccessLevel RoleAccessLevel { get; set; }
        public string Permission { get; set; }
        public string Category { get; set; }

    }
  
}
