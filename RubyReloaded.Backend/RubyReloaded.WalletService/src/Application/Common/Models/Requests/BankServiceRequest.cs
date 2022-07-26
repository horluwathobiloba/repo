using RubyReloaded.WalletService.Domain.Enums;

namespace RubyReloaded.WalletService.Application.Common.Models.Requests
{
    public class BankServiceRequest
    {
        public BankServiceCategory BankServiceCategory { get; set; }
        public decimal CommissionFee { get; set; }
        public FeeType CommissionFeeType { get; set; }
        public decimal MaximumTransactionLimit { get; set; }
        public decimal MinimumTransactionLimit { get; set; }
        public decimal TransactionFee { get; set; }
        public FeeType TransactionFeeType { get; set; }
        public Status Status { get; set; }

    }
}
