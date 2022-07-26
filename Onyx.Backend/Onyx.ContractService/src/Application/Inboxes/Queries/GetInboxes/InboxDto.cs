using AutoMapper;
using Onyx.ContractService.Application.Common.Mappings;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.Inboxes.Queries.GetInboxes
{
    public class InboxDto : IMapFrom<Domain.Entities.Inbox>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }
        public string Body { get; set; }
        public string ReciepientEmail { get; set; }
        public bool Delivered { get; set; }
        public bool Read { get; set; }
        public EmailAction EmailAction { get; set; }
        public string Sender { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Inbox, InboxDto>();
            profile.CreateMap<InboxDto, Domain.Entities.Inbox>();
        }
    }
}
