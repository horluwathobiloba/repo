using Onyx.ContractService.Domain.Common;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Domain.Entities
{
    public class ContractDuration:AuditableEntity
    {
        public int Duration { get; set; }
        public DurationFrequency DurationFrequency { get; set; }
    }

}
