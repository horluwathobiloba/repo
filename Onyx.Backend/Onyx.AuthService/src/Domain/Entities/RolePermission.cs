using Onyx.AuthService.Domain.Common;
using Onyx.AuthService.Domain.Enums;

using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.AuthService.Domain.Entities
{
    public class RolePermission : AuditableEntity
    {
        public int RoleId { get; set; }
        public int OrganizationId { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public string Permission { get; set; }
       
    }
  
}
