using Onyx.ContractService.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Onyx.ContractService.Domain.Entities
{
   public class ContractTemplate : AuditableEntity
    { 
        public int ContractTypeId { get; set; }
        public string TemplateDocument { get; set; }

        [ForeignKey(nameof(ContractTypeId))]
        public ContractType ContractType { get; set; }
    }
}
