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
    public class ContractComment : AuditableEntity
    {
        public int ContractId { get; set; }
        public string CommentById { get; set; }
        public ContractCommentType ContractCommentType { get; set; }
        public string Comment { get; set; }
        [ForeignKey(nameof(ContractId))]
        public Contract Contract { get; set; }
    }
}
