using Onyx.ContractService.Domain.Common;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Onyx.ContractService.Domain.Entities
{
    public class WorkflowLevel : AuditableEntity
    {
        public int WorkflowPhaseId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string WorkflowLevelAction { get; set; }
        public int Rank { get; set; }

        [ForeignKey(nameof(WorkflowPhaseId))]
        public WorkflowPhase WorkflowPhase { get; set; }
    }
}
