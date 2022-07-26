using OnyxDoc.SubscriptionService.Domain.Enums;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace OnyxDoc.SubscriptionService.Application.Common.Models
{
    public class UpdatePGProductRequest
    {
        public int Id { get; set; }
        public int SubscriptionPlanId { get; set; }
        public int PaymentGatewayProductId { get; set; }
        public string PaymentGatewayProductCode { get; set; }
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