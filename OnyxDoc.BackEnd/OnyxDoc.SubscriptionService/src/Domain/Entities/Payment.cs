using OnyxDoc.SubscriptionService.Domain.Common;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnyxDoc.SubscriptionService.Domain.Entities
{
    public class Payment : AuditableEntity
    {
        //
        // Summary:
        //     (ID of the Customer) The ID of the customer for this Session. For Checkout Sessions
        //     in payment or subscription mode, Checkout will create a new customer object based
        //     on information provided during the payment flow unless an existing customer was
        //     provided when the Session was created. 
         
        public int SubscriptionId { get; set; }
        public string SubscriptionNo { get; set; }
        public int PaymentChannelId { get; set; }
        public decimal Amount { get; set; }
        public long Quantity { get; set; }
        public string AuthorizationUrl { get; set; }
        public string PaymentMethodType { get; set; }
        public decimal FeeRate { get; set; }
        public decimal TransactionFee { get; set; }
        public string CurrencyCode { get; set; }
        public string SessionId { get; set; }
        public string ReferenceNo { get; set; }
        public string Mode { get; set; }
        public string ClientReferenceNo { get; set; }
        public string SuccessUrl { get; set; }
        public string CancelUrl { get; set; }
        public string Hash { get; set; }
        public string ProviderStatus { get; set; }
        public DateTime ProviderCreatedDate { get; set; }
        public string PaymentIntentId { get; set; }

        public PaymentStatus PaymentStatus { get; set; }
        public string PaymentStatusDesc { get; set; }
        public DateTime LogDate { get; set; }
        public DateTime CompletedDate { get; set; }
        public DateTime FailedDate { get; set; }
        public DateTime CancelledDate { get; set; }
        public DateTime ReversedDate { get; set; }
        public DateTime PaymentDate { get; set; }
         

        [ForeignKey(nameof(SubscriptionId))]
        public Subscription Subscription { get; set; }
    }
}
