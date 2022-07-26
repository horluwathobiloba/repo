using Onyx.WorkFlowService.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.WorkFlowService.Domain.Entities
{
    public class StaffCount : AuditableEntity
    {
        public int Count { get; set; }
        public int OrganizationId { get; set; }
    }
}
