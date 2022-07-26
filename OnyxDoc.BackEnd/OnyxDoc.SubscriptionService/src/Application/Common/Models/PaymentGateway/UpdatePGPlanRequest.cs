using OnyxDoc.SubscriptionService.Domain.Enums;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace OnyxDoc.SubscriptionService.Application.Common.Models
{
    public class UpdatePGPlanRequest
    {
        public int Id { get; set; }
        public int SubscriptionId { get; set; }
        public int PaymentGatewayPlanId { get; set; }
        public string PaymentGatewayPlanCode { get; set; }
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