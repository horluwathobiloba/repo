using OnyxDoc.SubscriptionService.Domain.Enums;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace OnyxDoc.SubscriptionService.Application.Common.Models
{
    public class CreatePGPriceRequest
    {
        public int SubscriptionPlanPricingId { get; set; } 
        public int PaymentGatewayPriceId { get; set; }
        public string PaymentGatewayPriceCode { get; set; } 
        public PaymentGateway PaymentGateway { get; set; }
    }
}