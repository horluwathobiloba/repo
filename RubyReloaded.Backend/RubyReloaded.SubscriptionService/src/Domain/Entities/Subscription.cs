using RubyReloaded.SubscriptionService.Domain.ViewModels;
using RubyReloaded.SubscriptionService.Domain.Common;
using RubyReloaded.SubscriptionService.Domain.Enums;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace RubyReloaded.SubscriptionService.Domain.Entities
{
    public class Subscription : AuditableEntity
    { 
        public int SubscriptionPlanId { get; set; } 
        public int? InitialSubscriptionPlanId { get; set; } 
        public int? RenewedSubscriptionPlanId { get; set; }
        public int CurrencyId { get; set; } 
        public int PaymentChannelId { get; set; }
        public string CurrencyCode { get; set; }
        public string TransactionReference { get; set; }
        public string PaymentChannelReference { get; set; }
        public string PaymentChannelResponse { get; set; }
        public string PaymentChannelStatus { get; set; }
        public decimal Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string PaymentStatusDesc { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
        public string SubscriptionTypeDesc { get; set; }

        [ForeignKey(nameof(SubscriptionPlanId))]
        public SubscriptionPlan  SubscriptionPlan { get; set; }

        [ForeignKey(nameof(CurrencyId))]
        public Currency Currency { get; set; }

        public SubscriberDto Subscriber { get; set; }

    }
}
