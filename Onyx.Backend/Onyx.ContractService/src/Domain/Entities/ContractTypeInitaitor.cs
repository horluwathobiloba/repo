using Onyx.ContractService.Domain.Common;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Onyx.ContractService.Domain.Entities
{
    public class ContractTypeInitiator : AuditableEntity
    {
        public int ContractTypeId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int JobFunctionId { get; set; }
        public string JobFunctionName { get; set; }

        [ForeignKey(nameof(ContractTypeId))]
        public ContractType ContractType { get; set; } 
    }
}
