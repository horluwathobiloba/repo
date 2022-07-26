using AutoMapper;
using OnyxDoc.SubscriptionService.Application.Common.Mappings;
using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace OnyxDoc.SubscriptionService.Application.PGPlans.Queries
{
    public class PGPlanDto : IMapFrom<PGPlan>
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public int SubscriptionId { get; set; }
        public int PaymentGatewayPlanId { get; set; }
        public string PaymentGatewayPlanCode { get; set; }
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
            profile.CreateMap<PGPlan, PGPlanDto>();
            profile.CreateMap<PGPlanDto, PGPlan>();
        }
    }
}
