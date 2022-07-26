using RubyReloaded.WalletService.Domain.Enums;

namespace RubyReloaded.WalletService.Application.Common.Models
{
    public class ServiceConfigVm
    {
        public ServiceCategory ServiceCategory { get; set; }
        public ServiceType ServiceType { get; set; }
        public string Biller { get; set; }
        public decimal MinAmount { get; set; }
        public decimal MaxAmount { get; set; }
        public bool IsCommissionFee { get; set; }
        public FeeType CommissionFeeType { get; set; }
        public decimal CommissionFee { get; set; }
        public bool IsTransactionFee { get; set; }
        public FeeType TransactionFeeType { get; set; }
        public decimal TransactionFee { get; set; }
        public string ServiceProvider { get; set; }
    }
}
