using AutoMapper;
using OnyxDoc.SubscriptionService.Application.Common.Mappings;
using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace OnyxDoc.SubscriptionService.Application.PGPrices.Queries
{
    public class PGPriceDto : IMapFrom<PGPrice>
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public int SubscriptionPlanPricingId { get; set; }
        public int PaymentGatewayPriceId { get; set; }
        public string PaymentGatewayPriceCode { get; set; }
        public PaymentGateway PaymentGateway { get; set; }

        public SubscriptionPlanPricing SubscriptionPlanPricing { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<PGPrice, PGPriceDto>();
            profile.CreateMap<PGPriceDto, PGPrice>();
        }
    }
}
