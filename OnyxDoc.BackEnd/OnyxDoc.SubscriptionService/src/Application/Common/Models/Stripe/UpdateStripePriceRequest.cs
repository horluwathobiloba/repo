using OnyxDoc.SubscriptionService.Domain.Entities; 

namespace OnyxDoc.SubscriptionService.Application.Common.Models
{
    public class UpdateStripePriceRequest
    {      
        public SubscriptionPlanPricing Price { get; set; }
    }
}
