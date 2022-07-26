using RubyReloaded.SubscriptionService.Domain.Enums;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace RubyReloaded.SubscriptionService.Application.Common.Models
{
    public class CreatePaymentChannelRequest
    {
        public int CurrencyId { get; set; }
        public string Name { get; set; }
        public CurrencyCode CurrencyCode { get; set; }
        public decimal TransactionFee { get; set; }
        public TransactionRateType TransactionRateType { get; set; }
    }
}