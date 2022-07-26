using Onyx.ContractService.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Domain.Entities
{
    public class ContractTag:AuditableEntity
    {
        public int ContractId { get; set; }
    }
}
