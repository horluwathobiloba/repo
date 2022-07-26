using RubyReloaded.WalletService.Domain.Common;
using RubyReloaded.WalletService.Domain.Entities;
using RubyReloaded.WalletService.Domain.Enums;
using System;

namespace RubyReloaded.WalletService.Domain.Entities
{
    public class Transaction : AuditableEntity
    {
        public int AccountId { get; set; }
        public string AccountNumber { get; set; }
        public TransactionServiceType TransactionCategory { get; set; }
        public string TransactionCategoryDesc { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime ValueDate { get; set; }
        public TransactionStatus TransactionStatus { get; set; }
        public string TransactionStatusDesc { get; set; }
        public TransactionType TransactionType { get; set; }
        public string TransactionTypeDesc { get; set; }
        public decimal Amount { get; set; }
        public CurrencyCode CurrencyCode { get; set; }
        public string CurrencyCodeDesc { get; set; }
        public Account Account { get; set; }
        public string TransactionReference { get; set; }
        public string ExternalTransactionReference { get; set; }
        public string RecipientName { get; set; }
        public string Narration { get; set; }
        public string RecipientProfilePicture { get; set; }
        public int PaymentChannelId { get; set; }
        public PaymentChannel PaymentChannel { get; set; }

    }
}
