using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Domain.ViewModels;

namespace OnyxDoc.SubscriptionService.Application.Common.Models
{
    public class CreateStripeSubscriptionRequest
    {
        public Subscription Subscription { get; set; }
        public SubscriptionPlanPricing Price { get; set; }
        public SubscriberDto Subscriber { get; set; }
    }
}
