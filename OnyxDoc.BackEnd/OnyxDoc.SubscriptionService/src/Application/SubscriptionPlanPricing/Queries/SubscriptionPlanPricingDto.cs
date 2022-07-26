using AutoMapper;
using OnyxDoc.SubscriptionService.Application.Common.Mappings;
using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace OnyxDoc.SubscriptionService.Application.SubscriptionPlanPricings.Queries
{
    public class SubscriptionPlanPricingDto : IMapFrom<SubscriptionPlanPricing>
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public int SubscriptionPlanId { get; set; }
        public string CurrencyCode { get; set; }
        public int CurrencyId { get; set; } 
        public decimal Amount { get; set; }
        public PricingPlanType PricingPlanType { get; set; }
        public string PricingPlanTypeDesc { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SubscriptionPlanPricing, SubscriptionPlanPricingDto>();
            profile.CreateMap<SubscriptionPlanPricingDto, SubscriptionPlanPricing>();
        }
    }
}
