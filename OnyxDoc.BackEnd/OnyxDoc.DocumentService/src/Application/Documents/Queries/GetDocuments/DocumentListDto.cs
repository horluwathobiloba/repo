
using AutoMapper;
using OnyxDoc.DocumentService.Application.Common.Mappings;
using OnyxDoc.DocumentService.Domain.Enums;
using System;
using System.Collections.Generic;

namespace OnyxDoc.DocumentService.Application.Document.Queries.GetDocument
{
    public class DocumentListDto : IMapFrom<Domain.Entities.Document>
{
        public string Name { get; set; }
        public string Description { get; set; }
        public string MimeType { get; set; }
        public string File { get; set; }
        public string SignedDocument { get; set; }
        public string Extension { get; set; }
        public string UserId { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string SenderEmail { get; set; }
        public DocumentType DocumentType { get; set; }
        public string DocumentTypeDesc { get; set; }
        public string DocumentSigningUrl { get; set; }
        public string Email { get; set; }
        public bool IsSigned { get; set; }
        public string Version { get; set; }
        public string NextActorEmail { get; set; }
        public string NextActorAction { get; set; }
        public int NextActorRank { get; set; }
        public DocumentStatus DocumentStatus { get; set; }
        public string DocumentStatusDesc { get; set; }
        public ICollection<Domain.Entities.Document> Document { get; set; }
        public string Hash { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Document, DocumentListDto> ();
            profile.CreateMap<DocumentListDto, Domain.Entities.Document>();
        }
    }
}
