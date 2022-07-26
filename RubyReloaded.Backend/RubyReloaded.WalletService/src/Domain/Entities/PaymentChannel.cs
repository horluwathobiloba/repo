 using RubyReloaded.WalletService.Domain.Common;
using RubyReloaded.WalletService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Domain.Entities
{
    public class PaymentChannel : AuditableEntity
    {
        public string Image { get; set; }
        public decimal PaymentGatewayFee { get; set; }
        public FeeType PaymentGatewayFeeType { get; set; }
        public string PaymentGatewayFeeTypeDesc { get; set; }
        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }
        public string CurrencyCode { get; set; }
        public string SettlementAccountNumber { get; set; }
        public int? BankId { get; set; }
        public Bank Bank { get; set; }
        public PaymentChannelType PaymentChannelType { get; set; }
        public string PaymentChannelTypeDesc { get; set; }
        public PaymentGatewayCategory PaymentGatewayCategory { get; set; }
        public string PaymentGatewayCategoryDesc { get; set; }
        public ICollection<BankService> BankServices { get; set; }
        public ICollection<PaymentGatewayService> PaymentGatewayServices { get; set; }
    }
}
