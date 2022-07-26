using OnyxDoc.SubscriptionService.Domain.Enums;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace OnyxDoc.SubscriptionService.Application.Common.Models
{
    public class UpdatePGCustomerRequest
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public int PaymentGatewayCustomerId { get; set; }
        public string PaymentGatewayCustomerCode { get; set; }
        public PaymentGateway PaymentGateway { get; set; }
        public bool IsDeleted { get; set; }

        public Status Status
        {
            get
            {
                return IsDeleted ? Status.Deactivated : Status.Active;
            }
        }
    }
}