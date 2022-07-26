using AutoMapper;
using OnyxDoc.DocumentService.Application.Common.Mappings;
using OnyxDoc.DocumentService.Domain.Entities;
using OnyxDoc.DocumentService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.DocumentService.Application.Recipients.Queries.GetRecipients
{
    public class RecipientDto : IMapFrom<Domain.Entities.Recipient>
    {
        public int Id { get; set; }
        public int DocumentId { get; set; }
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public int Rank { get; set; }
        public string RecipientCategory { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Recipient, RecipientDto>();
            profile.CreateMap<RecipientDto, Domain.Entities.Recipient>();
        }
    }
}
