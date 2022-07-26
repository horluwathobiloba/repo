using RubyReloaded.WalletService.Domain.Common;
using RubyReloaded.WalletService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Domain.Entities
{
    public class BankService : AuditableEntity
    {
        public BankServiceCategory BankServiceCategory { get; set; }
        public decimal MinimumTransactionLimit { get; set; }
        public decimal MaximumTransactionLimit { get; set; }
        public decimal TransactionFee { get; set; }
        public FeeType TransactionFeeType { get; set; }
        public decimal CommissionFee { get; set; }
        public FeeType CommissionFeeType { get; set; }
        public int PaymentChannelId { get; set; }
        public PaymentChannel PaymentChannel { get; set; }
    }
}
