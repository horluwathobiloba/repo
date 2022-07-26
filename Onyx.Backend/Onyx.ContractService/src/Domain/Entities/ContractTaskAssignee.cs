using Onyx.ContractService.Domain.Common;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.ContractService.Domain.Entities
{
    public class ContractTaskAssignee : AuditableEntity
    {
        public int ContractTaskId { get; set; }
        public string AssignedToId { get; set; }
        public string AssignedToName { get; set; }
        public string AssignedToEmail { get; set; }       

        [ForeignKey(nameof(ContractTaskId))]
        public ContractTask ContractTask { get; set; }
    }
}
