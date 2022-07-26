using OnyxDoc.DocumentService.Domain.Common;
using OnyxDoc.DocumentService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.DocumentService.Domain.Entities
{
    public class DocumentFeedback : AuditableEntity
    {
        public string UserId { get; set; }
        public int DocumentId { get; set; }
        public Rating Rating { get; set; }
        public string RatingDesc { get; set; }
        public string Comment { get; set; }
    }
}
