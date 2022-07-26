using OnyxDoc.SubscriptionService.Domain.Common;
using OnyxDoc.SubscriptionService.Domain.Enums;
using OnyxDoc.SubscriptionService.Domain.ViewModels;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnyxDoc.SubscriptionService.Domain.Entities
{
    public class PGPlan : AuditableEntity
    { 
        public string PaymentGatewayPlanCode { get; set; }
        public int PaymentGatewayPlanId { get; set; }
        public int SubscriptionId { get; set; }
        public PaymentGateway PaymentGateway { get; set; }
        public string PaymentGatewayDesc { get; set; }

        [ForeignKey(nameof(SubscriptionId))]
        public Subscription Subscription { get; set; }
    }
}
