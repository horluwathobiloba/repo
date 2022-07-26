using AutoMapper;
using OnyxDoc.DocumentService.Application.Common.Mappings;
using OnyxDoc.DocumentService.Domain.Entities;
using OnyxDoc.DocumentService.Domain.Enums;
using System;
using System.Collections.Generic;

namespace OnyxDoc.DocumentService.Application.Inboxes.Queries.GetInboxes
{
    public class InboxDto : IMapFrom<Inbox>
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Email { get; set; }
        public List<string> RecipientNames { get; set; }
        public EmailAction EmailAction { get; set; }
        public string DocumentUrl { get; set; }
        public string SenderEmail { get; set; }
        public bool Read { get; set; }
        public string Sender { get; set; }
        public int DocumentId { get; set; }


        public string CreatedBy { get; set; }
        public string CreatedByEmail { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Inbox, InboxDto>().ReverseMap();
        }
    }
}
