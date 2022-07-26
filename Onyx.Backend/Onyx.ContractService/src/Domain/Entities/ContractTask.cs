using Onyx.ContractService.Domain.Common;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Onyx.ContractService.Domain.Entities
{
    public class ContractTask : AuditableEntity
    {
        public int ContractId { get; set; }
        public DateTime DueDate { get; set; }
        public string TaskCreatedById { get; set; }
        public string AssignedUserId { get; set; }
        public string AssignedUserEmail { get; set; }
        public string TaskCompletedById { get; set; }
        public string TaskCompletedByName { get; set; }
        public string TaskCompletedByEmail { get; set; }
        public ContractTaskStatus ContractTaskStatus { get; set; }
        public string ContractTaskStatusDesc { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(ContractId))]
        public Contract Contract { get; set; }
    }
}
