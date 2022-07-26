using OnyxDoc.DocumentService.Domain.Common;
using OnyxDoc.DocumentService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OnyxDoc.DocumentService.Domain.Entities
{
    public class Document : AuditableEntity
    { 
        public string MimeType { get; set; }
        public string SubscriberType { get; set; }
        public DocumentType DocumentType { get; set; }
        public string DocumentTypeDesc { get; set; }
        public string File { get; set; }
        public string SignedDocument { get; set; }
        public string Extension { get; set; }
        public string UserId { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string SenderEmail { get; set; }
        public string DocumentSigningUrl { get; set; }
        public string Email { get; set; }
        public bool IsSigned { get; set; }
        public string Version { get; set; }
        public string NextActorEmail { get; set; }
        public string NextActorAction { get; set; }
        public string DocumentMessage { get; set; }
        public int NextActorRank { get; set; }
        public DocumentStatus DocumentStatus { get; set; }
        public string DocumentStatusDesc { get; set; }
        public ICollection<Recipient> Recipients { get; set; }
        public string Hash { get; set; }


        //Just Added
        public int? FolderId { get; set; }
        public bool IsDuplicated { get; set; }
        public bool IsArchived { get; set; }
        public bool IsDeleted { get; set; }
    }
}
