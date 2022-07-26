using RubyReloaded.WalletService.Domain.Entities;
using RubyReloaded.WalletService.Domain.Enums;
using System;

namespace RubyReloaded.WalletService.Application.Common.Models.Requests
{
    public class TransactionRequest
    {
        public int AccountId { get; set; }
        public string CustomerId { get; set; }
        public string Email { get; set; }
        public int AccountProductId { get; set; }
        public string AccountProductNumber { get; set; }
        public int ProductId { get; set; }
        public string AccountNumber { get; set; }
        public TransactionServiceType TransactionCategory { get; set; }
        public TransactionStatus TransactionStatus { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal Amount { get; set; }
        public CurrencyCode CurrencyCode { get; set; }
        public string TransactionReference { get; set; }
        public string ExternalTransactionReference { get; set; }
        public string Narration { get; set; }
        public int PaymentChannelId { get; set; }

    }
}
