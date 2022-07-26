using AutoMapper;
using OnyxDoc.SubscriptionService.Application.Common.Mappings;
using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace OnyxDoc.SubscriptionService.Application.Subscriptions.Queries
{
    public class SubscriptionDto : IMapFrom<Subscription>
    {
        public int Id { get; set; }
        public int SubscriptionPlanId { get; set; }
        public string SubscriptionNo { get; set; }
        public int? InitialSubscriptionPlanId { get; set; }
        public int? RenewedSubscriptionPlanId { get; set; }
        public int CurrencyId { get; set; }
        public int PaymentChannelId { get; set; }
        public string CurrencyCode { get; set; }
        public string TransactionReference { get; set; }
        public string PaymentChannelReference { get; set; }
        public string PaymentChannelResponse { get; set; }
        public string PaymentChannelStatus { get; set; }
        public string SubscriberName { get; set; }

        public decimal Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string PaymentStatusDesc { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
        public string SubscriptionTypeDesc { get; set; }
        public SubscriptionFrequency SubscriptionFrequency { get; set; }
        public string SubscriptionFrequencyDesc { get; set; }

        public string UserId { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByEmail { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Subscription, SubscriptionDto>();
            profile.CreateMap<SubscriptionDto, Subscription>();
        }
    }
}
