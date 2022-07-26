using OnyxDoc.SubscriptionService.Domain.Entities;

namespace OnyxDoc.SubscriptionService.Application.Common.Models
{
    public class UpdateStripeSubscriptionRequest
    {
        public Subscription Subscription { get; set; }
        public SubscriptionPlanPricing Price { get; set; }
    }
}
