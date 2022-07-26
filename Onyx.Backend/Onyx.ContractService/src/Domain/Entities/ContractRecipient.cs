using Onyx.ContractService.Domain.Common;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.ContractService.Domain.Entities
{
    public class ContractRecipient : AuditableEntity
    {
        public int ContractId { get; set; }
        public string Email { get; set; }
        public string Designation { get; set; }
        public int Rank { get; set; }
        public string RecipientCategory { get; set; }
        public string DocumentSigningUrl { get; set; }

       [ForeignKey(nameof(ContractId))]
        public Contract Contract { get; set; }

        public ICollection<ContractRecipientAction> ContractRecipientActions { get; set; } = new List<ContractRecipientAction>();
    }
}
