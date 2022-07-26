using OnyxDoc.SubscriptionService.Domain.ViewModels;
using OnyxDoc.SubscriptionService.Domain.Common;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnyxDoc.SubscriptionService.Domain.Entities
{
    public class Subscription : AuditableEntity
    {
        public int SubscriptionPlanId { get; set; }
        public string PaymentGatewaySubscriptionId { get; set; }
        public int SubscriptionPlanPricingId { get; set; }        
        public int? InitialSubscriptionPlanId { get; set; }
        public int? RenewedSubscriptionPlanId { get; set; }
        public bool FreeTrialActivated { get; set; }
        public int CurrencyId { get; set; }
        public int? PaymentChannelId { get; set; }
        public string CurrencyCode { get; set; }
        public string SubscriptionNo { get; set; }
        public DateTime TrialEndDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
        public string SubscriptionTypeDesc { get; set; }
        public SubscriptionFrequency SubscriptionFrequency { get; set; }
        public string SubscriptionFrequencyDesc { get; set; }
        public int PaymentPeriod { get; set; }

        public int? NumberOfUsers { get; set; }
        public decimal Amount { get; set; }
        public string UploadedReceipt { get; set; }
        /// <summary>
        /// TotalAmount = Amount * NumberOfUsers (Inclusive of TransactionFee)
        /// </summary>
        public decimal TotalAmount { get; set; }
        public decimal TransactionFee { get; set; }

        public string TransactionReference { get; set; }
        public string PaymentChannelReference { get; set; }
        public string PaymentChannelResponse { get; set; }
        public string PaymentChannelStatus { get; set; }

        public PaymentStatus PaymentStatus { get; set; }
        public string PaymentStatusDesc { get; set; }

        public bool CancelSubscriptionRenewal { get; set; }
        public string CancelReason { get; set; }

        public SubscriptionStatus SubscriptionStatus { get; set; }
        public string SubscriptionStatusDesc { get; set; }

        [ForeignKey(nameof(SubscriptionPlanPricingId))]
        public SubscriptionPlanPricing SubscriptionPlanPricing { get; set; }

        [ForeignKey(nameof(PaymentChannelId))]
        public PaymentChannel PaymentChannel { get; set; }

        [ForeignKey(nameof(SubscriptionPlanId))]
        public SubscriptionPlan SubscriptionPlan { get; set; }

        [ForeignKey(nameof(CurrencyId))]
        public Currency Currency { get; set; }

        [NotMapped]
        public SubscriberDto Subscriber { get; set; }

    }
}
