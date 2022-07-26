using AutoMapper;
using OnyxDoc.DocumentService.Application.Common.Mappings;
using OnyxDoc.DocumentService.Application.Recipients.Queries.GetRecipients;
using OnyxDoc.DocumentService.Domain.Entities;
using OnyxDoc.DocumentService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.DocumentService.Application.RecipientActions.Queries.GetRecipientActions
{
    public class RecipientActionDto : IMapFrom<RecipientAction>
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; } 
        public int DocumentId { get; set; }
        public int RecipientId { get; set; }
        public string RecipientAction { get; set; }
        public string AppSigningUrl { get; set; }
        public string SignedDocumentUrl { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }
        public RecipientDto Recipient { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<RecipientAction, RecipientActionDto>();
            profile.CreateMap<RecipientActionDto, RecipientAction>();
            profile.CreateMap<Domain.Entities.Recipient, RecipientDto>();
            profile.CreateMap<RecipientDto, Domain.Entities.Recipient>();
        }
    }
}
