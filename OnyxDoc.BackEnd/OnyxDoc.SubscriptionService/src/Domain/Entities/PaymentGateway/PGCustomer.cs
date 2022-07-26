using OnyxDoc.SubscriptionService.Domain.Common;
using OnyxDoc.SubscriptionService.Domain.Enums;
using OnyxDoc.SubscriptionService.Domain.ViewModels;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnyxDoc.SubscriptionService.Domain.Entities
{
    public class PGCustomer : AuditableEntity
    { 
        public string PaymentGatewayCustomerCode { get; set; }
        public int PaymentGatewayCustomerId { get; set; }
        public PaymentGateway PaymentGateway { get; set; }
        public string PaymentGatewayDesc { get; set; } 

        [NotMapped] 
        public SubscriberDto Subscriber { get; set; }
    }
}
