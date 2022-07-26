using Onyx.WorkFlowService.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.WorkFlowService.Domain.Entities
{
    public class Organization : AuditableEntity
    {
        public string RCNumber { get; set; }

        public string Address { get; set; }

        public string ContactEmail { get; set; }

        public string ContactPhoneNumber { get; set; }

        public string Code { get; set; }

        public string LogoFileLocation { get; set; }

        public bool GenerateStaffCode { get; set; }

        public string Country { get; set; }
        public string State { get; set; } 
    }
}
