using OnyxDoc.SubscriptionService.Domain.Enums;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace OnyxDoc.SubscriptionService.Application.Common.Models
{
    public class CreatePGSubscriptionRequest
    {
        public int SubscriptionId { get; set; }
        public int PaymentGatewaySubscriptionId { get; set; }
        public string PaymentGatewaySubscriptionCode { get; set; }
        public PaymentGateway PaymentGateway { get; set; }
    }
}