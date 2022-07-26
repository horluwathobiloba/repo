using Onyx.ContractService.Domain.Common;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Onyx.ContractService.Domain.Entities
{
    public class ContractType : AuditableEntity
    {       

        [DefaultValue(false)]
        public bool EnableInternalSignatories { get; set; }
        [DefaultValue(0)]
        public int NumberOfInternalSignatories { get; set; }

        [DefaultValue(false)]
        public bool EnableExternalSignatories { get; set; }
        public int NumberOfExternalSignatories { get; set; }

        [DefaultValue(false)]
        public bool EnableThirdPartySignatories { get; set; }
        [DefaultValue(0)]
        public int NumberOfThirdPartySignatories { get; set; }

        [DefaultValue(false)]
        public bool EnableWitnessSignatories { get; set; }
        [DefaultValue(0)]
        public int NumberOfWitnessSignatories { get; set; }

        public string TemplateFilePath { get; set; }

        public string ContractTypeStatus { get; set; }

        [DefaultValue(false)]
        public bool EnableContractTypeInitiatorRestriction { get; set; }

       

        public ICollection<ContractTypeInitiator> ContractTypeInitiators { get; set; } = new List<ContractTypeInitiator>();
        public ICollection<WorkflowPhase> WorkflowPhases { get; set; } = new List<WorkflowPhase>();

    }
}
