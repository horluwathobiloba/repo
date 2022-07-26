using OnyxDoc.DocumentService.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.DocumentService.Domain.Entities
{
    public class ContactFeedback : AuditableEntity
    {
        public string Email { get; set; }
        public string ContactMessage { get; set; }
    }
}
