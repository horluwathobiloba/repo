using OnyxDoc.SubscriptionService.Domain.Enums;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace OnyxDoc.SubscriptionService.Application.Common.Models
{
    public class UpdatePGPriceRequest
    {
        public int Id { get; set; }
        public int SubscriptionPlanPricingId { get; set; }
        public int PaymentGatewayPriceId { get; set; }
        public string PaymentGatewayPriceCode { get; set; }
        public PaymentGateway PaymentGateway { get; set; }
        public bool IsDeleted { get; set; }

        public Status Status
        {
            get
            {
                return IsDeleted ? Status.Deactivated : Status.Active;
            }
        }
    }
}