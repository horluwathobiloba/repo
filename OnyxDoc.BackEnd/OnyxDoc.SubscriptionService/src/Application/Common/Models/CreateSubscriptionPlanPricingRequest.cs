using OnyxDoc.SubscriptionService.Domain.Enums;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace OnyxDoc.SubscriptionService.Application.Common.Models
{
    public class CreateSubscriptionPlanPricingRequest
    {
        public int SubscriptionPlanId { get; set; }
        public string CurrencyCode { get; set; }
        public int CurrencyId { get; set; }
        public PricingPlanType PricingPlanType { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; } 
    }
}