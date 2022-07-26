using OnyxDoc.SubscriptionService.Domain.Common;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnyxDoc.SubscriptionService.Domain.Entities
{
    public class PGPrice : AuditableEntity
    {
        public int SubscriptionPlanPricingId { get; set; }
        public int PaymentGatewayPriceId { get; set; } 
        public string PaymentGatewayPriceCode { get; set; }
        public PaymentGateway PaymentGateway { get; set; }
        public string PaymentGatewayDesc { get; set; }

        [ForeignKey(nameof(SubscriptionPlanPricingId))]
        public SubscriptionPlanPricing SubscriptionPlanPricing { get; set; }
    }
}
