using OnyxDoc.SubscriptionService.Domain.Common;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnyxDoc.SubscriptionService.Domain.Entities
{
    public class PGProduct : AuditableEntity
    {
        public int SubscriptionPlanId { get; set; }
        public int PaymentGatewayProductId { get; set; }
        public string PaymentGatewayProductCode { get; set; }
        public PaymentGateway PaymentGateway { get; set; }
        public string PaymentGatewayDesc { get; set; }
        [ForeignKey(nameof(SubscriptionPlanId))]
        public SubscriptionPlan SubscriptionPlan { get; set; }
    }
}
