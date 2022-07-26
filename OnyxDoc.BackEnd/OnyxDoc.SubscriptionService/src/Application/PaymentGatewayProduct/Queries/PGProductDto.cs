using AutoMapper;
using OnyxDoc.SubscriptionService.Application.Common.Mappings;
using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace OnyxDoc.SubscriptionService.Application.PGProducts.Queries
{
    public class PGProductDto : IMapFrom<PGProduct>
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public int SubscriptionPlanId { get; set; }
        public int PaymentGatewayProductId { get; set; }
        public string PaymentGatewayProductCode { get; set; }
        public PaymentGateway PaymentGateway { get; set; }

        public SubscriptionPlan SubscriptionPlan { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<PGProduct, PGProductDto>();
            profile.CreateMap<PGProductDto, PGProduct>();
        }
    }
}
