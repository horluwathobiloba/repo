using RubyReloaded.WalletService.Domain.Common;
using RubyReloaded.WalletService.Domain.Entities;
using RubyReloaded.WalletService.Domain.Enums;

namespace RubyReloaded.WalletService.Domain.Entities
{
    public class WalletTransaction: AuditableEntity
    {

        public string UserId { get; set; }
        public string AccountHolder { get; set; }
        public ServiceCategory ServiceCategory { get; set; }
        public TransactionType TransactionType { get; set; }
        public TransactionStatus TransactionStatus { get; set; }
        public string TransactionStatusDesc { get; set; }
        public string TransactionTypeDesc { get; set; }
        public decimal TransactionAmount { get; set; }
        public int WalletId { get; set; }
        public CurrencyCode CurrencyCode { get; set; }
        public string CurrencyCodeDesc { get; set; }
        public Wallet Wallet { get; set; }
        public string Hash { get; set; }
        public string TransactionReference { get; set; }
        public string ExternalTransactionReference { get; set; }
        public string ReciepientName { get; set; }
        public string ReciepientProfilePicture { get; set; }
        public int PaymentChannelId { get; set; }
        public PaymentChannel PaymentChannel { get; set; }

    }
}
