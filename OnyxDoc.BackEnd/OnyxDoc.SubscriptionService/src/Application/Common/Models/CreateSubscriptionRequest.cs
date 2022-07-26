using OnyxDoc.SubscriptionService.Application.Subscriptions.Commands;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace OnyxDoc.SubscriptionService.Application.Common.Models
{
    public class CreateSubscriptionRequest
    {
        public int SubscriberId { get; set; }
        public int SubscriptionPlanPricingId { get; set; }
        public string SubscriberName { get; set; }
        public int SubscriptionPlanId { get; set; }
        public int? InitialSubscriptionPlanId { get; set; }
        public int? RenewedSubscriptionPlanId { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
        public int CurrencyId { get; set; }
        public int PaymentChannelId { get; set; }
        public string CurrencyCode { get; set; }
        public SubscriptionFrequency SubscriptionFrequency { get; set; }
        public int PaymentPeriod { get; set; }
        public int NumberOfUsers { get; set; }
        public decimal Amount { get; set; }

        public bool IsFreeTrial { get; set; }

        public string UserId { get; set; }      

    }
}