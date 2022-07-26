using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Domain.Enums;

namespace OnyxDoc.SubscriptionService.Application.Common.Models
{
    public class UpdatePaystackSubscriptionRequest
    {
        public Subscription Subscription { get; set; }
        public SubscriptionPlanPricing Price { get; set; }
        public PaymentGateway PaymentGateway { get; set; }
        public string UserId { get; set; }
    }
}
