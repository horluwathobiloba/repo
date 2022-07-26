using OnyxDoc.SubscriptionService.Domain.Entities;

namespace OnyxDoc.SubscriptionService.Application.Common.Models
{
    public class CreatePaystackPlanRequest
    {
        public Subscription Plan { get; set; }
    }
}
