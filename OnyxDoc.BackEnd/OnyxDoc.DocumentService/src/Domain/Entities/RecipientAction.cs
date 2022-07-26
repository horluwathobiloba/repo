using OnyxDoc.DocumentService.Domain.Common;
using OnyxDoc.DocumentService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OnyxDoc.DocumentService.Domain.Entities
{
    public class RecipientAction : AuditableEntity
    {
        public int DocumentId { get; set; }
        public int RecipientId { get; set; }
        public DocumentRecipientAction DocumentRecipientAction { get; set; }
        public string RecipientActionDesc { get; set; }
        public string AppSigningUrl { get; set; }
        public string SignedDocumentUrl { get; set; }
        [ForeignKey(nameof(RecipientId))]
        public Recipient Recipient { get; set; }
    }
}
