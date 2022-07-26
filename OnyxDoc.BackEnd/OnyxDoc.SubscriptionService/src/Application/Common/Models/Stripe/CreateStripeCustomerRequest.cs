using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Domain.ViewModels;

namespace OnyxDoc.SubscriptionService.Application.Common.Models
{
    public class CreateStripeCustomerRequest
    {
        public SubscriberDto Subscriber { get; set; }
    }
}
