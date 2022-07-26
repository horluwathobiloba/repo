using OnyxDoc.SubscriptionService.Domain.Enums;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace OnyxDoc.SubscriptionService.Application.Common.Models
{
    public class CreatePaymentChannelRequest
    {
        public int CurrencyId { get; set; }
        public string Name { get; set; }
        public decimal TransactionFee { get; set; }
        public PaymentGateway PaymentGateway { get; set; }

        public CurrencyCode CurrencyCode { get; set; }
        public CurrencyCode CurrencyCodeDesc { get; set; }
        public TransactionRateType TransactionRateType { get; set; }
    }
}