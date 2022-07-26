using OnyxDoc.SubscriptionService.Domain.Entities; 

namespace OnyxDoc.SubscriptionService.Application.Common.Models
{
    public class UpdateStripeProductRequest
    {      
        public SubscriptionPlan Plan { get; set; }
    }
}
