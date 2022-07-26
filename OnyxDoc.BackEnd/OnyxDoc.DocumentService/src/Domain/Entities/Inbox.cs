using OnyxDoc.DocumentService.Domain.Common;
using OnyxDoc.DocumentService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OnyxDoc.DocumentService.Domain.Entities
{
    public class Inbox : AuditableEntity
    {
        public string Subject { get; set; }
        public string Email { get; set; }
        public List<string> RecipientNames { get; set; }
        public EmailAction EmailAction { get; set; }
        public string DocumentUrl { get; set; }
        public string SenderEmail { get; set; }
        public bool Read { get; set; }
        public string Sender { get; set; }
        public Document Document { get; set; }
        public int DocumentId { get; set; }
    }
}
