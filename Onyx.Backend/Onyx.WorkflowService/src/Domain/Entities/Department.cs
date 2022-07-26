using Onyx.WorkFlowService.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.WorkFlowService.Domain.Entities
{
    public class Department : AuditableEntity
    {
        public int OrganizationId { get; set; }
    }
}
