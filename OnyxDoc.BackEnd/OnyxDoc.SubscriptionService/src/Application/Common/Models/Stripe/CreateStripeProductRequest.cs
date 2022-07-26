using OnyxDoc.SubscriptionService.Domain.Entities; 

namespace OnyxDoc.SubscriptionService.Application.Common.Models
{
    public class CreateStripeProductRequest
    {
        public SubscriptionPlan Plan { get; set; }
    }
}
