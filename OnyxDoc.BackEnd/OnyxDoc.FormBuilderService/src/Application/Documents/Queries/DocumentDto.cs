using AutoMapper;
using OnyxDoc.FormBuilderService.Application.Common.Mappings;
using OnyxDoc.FormBuilderService.Application.DocumentPages.Queries;
using OnyxDoc.FormBuilderService.Domain.Entities;
using OnyxDoc.FormBuilderService.Domain.Enums;
using System;
using System.Collections.Generic;

namespace OnyxDoc.FormBuilderService.Application.Documents.Queries
{
    public class DocumentDto : IMapFrom<Document>
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public string Name { get; set; }
        public int InitialDocumentVersionId { get; set; }
        public DocumentType DocumentType { get; set; }
        public string DocumentTypeDesc { get; set; }
        public DocumentShareType DocumentShareType { get; set; }
        public string DocumentShareTypeDesc { get; set; }
        public DocumentStatus DocumentStatus { get; set; }
        public string DocumentStatusDesc { get; set; }

        public string UserId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }

        public List<DocumentPageDto> DocumentPages { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Document, DocumentDto>();
            profile.CreateMap<DocumentDto, Document>();

            profile.CreateMap<DocumentPage, DocumentPageDto>();
            profile.CreateMap<DocumentPageDto, DocumentPage>();
        }
    }
}
