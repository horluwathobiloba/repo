using Onyx.ContractService.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.ContractService.Domain.Entities
{
    public class SupportingDocument : AuditableEntity
    {
        public int ContractId { get; set; }
        public string MimeType { get; set; }
        public string File { get; set; }
        public string Extension { get; set; }
        [ForeignKey(nameof(ContractId))]
        public Contract Contract { get; set; }

    }
}
