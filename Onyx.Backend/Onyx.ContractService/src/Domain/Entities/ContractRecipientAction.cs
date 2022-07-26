using Onyx.ContractService.Domain.Common;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.ContractService.Domain.Entities
{
    public class ContractRecipientAction : AuditableEntity
    {
        public int ContractId { get; set; }
        public int ContractRecipientId { get; set; }
        public string ApproverSignatureMimeType { get; set; }
        public string ApproverSignatureFileExtension { get; set; }
        public string ApproverSignatureBlobFileUrl { get; set; }

        public string RecipientAction { get; set; }
        public string AppSigningUrl { get; set; }
        public string SignedDocumentUrl { get; set; }

        [ForeignKey(nameof(ContractRecipientId))]
        public ContractRecipient ContractRecipient { get; set; } 
    }
}
