using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Domain.Enums;

namespace OnyxDoc.SubscriptionService.Application.Common.Models
{
    public class UpdatePaystackPlanRequest
    {      
        public Subscription Subscription { get; set; }
        public PaymentGateway PaymentGateway { get; set; }
        public string UserId { get; set; }
    }
}
