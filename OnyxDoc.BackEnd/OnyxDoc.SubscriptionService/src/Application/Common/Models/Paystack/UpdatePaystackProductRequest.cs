using MediatR;
using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Domain.Enums;

namespace OnyxDoc.SubscriptionService.Application.Common.Models
{
    public class UpdatePaystackProductRequest
    {
        public PaymentGateway PaymentGateway { get; set; }
        public SubscriptionPlan Plan { get; set; }
        public SubscriptionPlanPricing Price { get; set; }
        public string UserId { get; set; }
    }
}
