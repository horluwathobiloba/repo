using Onyx.ContractService.Domain.Common;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.ContractService.Domain.Entities
{
    public class ReminderRecipient : AuditableEntity
    {
        public int ContractId { get; set; }
        public string Email { get; set; }

        [ForeignKey(nameof(ContractId))]
        public Contract Contract { get; set; }
    }
}
