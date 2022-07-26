using OnyxDoc.SubscriptionService.Domain.Enums;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace OnyxDoc.SubscriptionService.Application.Common.Models
{
    public class CreatePGPlanRequest
    {
        public int SubscriptionId { get; set; }
        public int PaymentGatewayPlanId { get; set; }
        public string PaymentGatewayPlanCode { get; set; }
        public PaymentGateway PaymentGateway { get; set; }
    }
}