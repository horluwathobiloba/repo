using RubyReloaded.AuthService.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Domain.Entities
{
    public class PaymentChannel:AuditableEntity
    {
        public decimal TransactionFee { get; set; }
        public TransactionFeeType TransactionFeeType { get; set; }
        public int CurrencyConfigurationId { get; set; }
        public CurrencyConfiguration Currency { get; set; }
        public string SettlementAccountNumber { get; set; }
        public string Bank { get; set; }
    }
}
