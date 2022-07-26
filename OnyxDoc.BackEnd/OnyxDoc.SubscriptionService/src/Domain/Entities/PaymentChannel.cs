using OnyxDoc.SubscriptionService.Domain.Common;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnyxDoc.SubscriptionService.Domain.Entities
{
    public class PaymentChannel : AuditableEntity
    {
       
        public PaymentGateway PaymentGateway { get; set; }
        public string PaymentGatewayDesc { get; set; }
        public int CurrencyId { get; set; }
        public CurrencyCode CurrencyCode { get; set; }
        public string CurrencyCodeDesc { get; set; }
        public decimal TransactionFee { get; set; }
        public TransactionRateType TransactionRateType { get; set; }
        public string TransactionRateTypeDesc { get; set; } 
        [ForeignKey(nameof(CurrencyId))]
        public Currency Currency { get; set; } 
    }
}
