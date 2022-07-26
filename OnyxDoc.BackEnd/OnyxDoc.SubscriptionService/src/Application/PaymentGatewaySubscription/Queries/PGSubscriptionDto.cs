using AutoMapper;
using OnyxDoc.SubscriptionService.Application.Common.Mappings;
using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace OnyxDoc.SubscriptionService.Application.PGSubscriptions.Queries
{
    public class PGSubscriptionDto : IMapFrom<PGSubscription>
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public int SubscriptionId { get; set; }
        public int PaymentGatewaySubscriptionId { get; set; }
        public string PaymentGatewaySubscriptionCode { get; set; }
        public PaymentGateway PaymentGateway { get; set; }

        public Subscription Subscription { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<PGSubscription, PGSubscriptionDto>();
            profile.CreateMap<PGSubscriptionDto, PGSubscription>();
        }
    }
}
