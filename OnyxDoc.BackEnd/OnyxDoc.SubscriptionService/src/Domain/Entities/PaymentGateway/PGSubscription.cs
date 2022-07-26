using OnyxDoc.SubscriptionService.Domain.Common;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnyxDoc.SubscriptionService.Domain.Entities
{
    public class PGSubscription : AuditableEntity
    {
        public int SubscriptionId { get; set; }
        public string PaymentGatewaySubscriptionCode { get; set; }
        public int PaymentGatewaySubscriptionId { get; set; }

        public PaymentGateway PaymentGateway { get; set; }
        public string PaymentGatewayDesc { get; set; }
        [ForeignKey(nameof(SubscriptionId))]
        public Subscription Subscription { get; set; }
    }
}
