using Onyx.ContractService.Domain.Common;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Onyx.ContractService.Domain.Entities
{
    public class WorkflowPhase : AuditableEntity
    {
        public int ContractTypeId { get; set; }
        public string WorkflowUserCategory { get; set; }
        public string WorkflowSequence { get; set; }

        [ForeignKey(nameof(ContractTypeId))]
        public ContractType ContractType { get; set; }

        public ICollection<WorkflowLevel> WorkflowLevels { get; set; } = new List<WorkflowLevel>();
    }
}
