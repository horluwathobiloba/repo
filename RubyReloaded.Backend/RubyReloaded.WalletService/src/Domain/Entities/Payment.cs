using RubyReloaded.WalletService.Domain.Common;
using RubyReloaded.WalletService.Domain.Enums;
using System;

namespace RubyReloaded.WalletService.Domain.Entities
{
    public class Payment : AuditableEntity
    {
        public decimal Amount { get; set; }
        public long Quantity { get; set; }
        public string AuthorizationUrl { get; set; }
        public string PaymentMethodType { get; set; }
        public decimal FeeRate { get; set; }
        public decimal TransactionFee { get; set; }
        public string CurrencyCode { get; set; }
        public string SessionId { get; set; }
        public string Mode { get; set; }
        public string ClientReferenceId { get; set; }
        public string SuccessUrl { get; set; }
        public string CancelUrl { get; set; }
        public string Hash { get; set; }
        public string ProviderStatus { get; set; }
        public DateTime ProviderCreatedDate { get; set; }
        public string PaymentIntentId { get; set; }
        public string WalletAccountNumber { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string PaymentStatusDesc { get; set; }
        public DateTime LogDate { get; set; }
        public DateTime CompletedDate { get; set; }
        public DateTime FailedDate { get; set; }
        public DateTime CancelledDate { get; set; }
        public DateTime ReversedDate { get; set; }
        public DateTime PaymentDate { get; set; }

    }
}