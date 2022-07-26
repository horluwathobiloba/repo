using Onyx.WorkFlowService.Domain.Common;
using Onyx.WorkFlowService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.WorkFlowService.Domain.Entities
{
    public class RolePermission : AuditableEntity
    {
        public int RoleId { get; set; }
        public int OrganizationId { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public string Permission { get; set; }
    }
  
}
