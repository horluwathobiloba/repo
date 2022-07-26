using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Domain.Enums;
using OnyxDoc.SubscriptionService.Domain.ViewModels;

namespace OnyxDoc.SubscriptionService.Application.Common.Models
{
    public class UpdatePaystackCustomerRequest
    {      
        public SubscriberDto Subscriber { get; set; }
        public PaymentGateway PaymentGateway { get; set; } 
        public string UserId { get; set; }
    }
}
