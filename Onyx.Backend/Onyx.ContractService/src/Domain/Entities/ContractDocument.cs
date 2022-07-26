using Onyx.ContractService.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.ContractService.Domain.Entities
{
    public class ContractDocument : AuditableEntity
    {
        public int ContractId { get; set; }
        public string MimeType { get; set; }
        public string File { get; set; }
        public string Extension { get; set; }
        public string UserId { get; set; }
        public string SenderEmail { get; set; }
        public string DocumentSigningUrl { get; set; }
        public string Email { get; set; }
        public bool IsSigned { get; set; }
        public string Version { get; set; }
        public string Hash { get; set; }

        [ForeignKey(nameof(ContractId))]
        public Contract Contract { get; set; }

    }
}
