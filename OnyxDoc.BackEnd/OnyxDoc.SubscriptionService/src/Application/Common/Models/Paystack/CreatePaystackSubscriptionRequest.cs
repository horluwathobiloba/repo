using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Domain.ViewModels;

namespace OnyxDoc.SubscriptionService.Application.Common.Models
{
    public class CreatePaystackSubscriptionRequest
    {

        public string PaystackCustomerCode { get; set; }
        public string PaystackPlanCode { get; set; }
        public Subscription Subscription { get; set; }   
        public SubscriberDto Subscriber { get; set; }
    }
}
