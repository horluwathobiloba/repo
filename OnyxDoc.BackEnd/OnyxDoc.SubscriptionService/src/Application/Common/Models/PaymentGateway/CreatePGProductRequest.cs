using OnyxDoc.SubscriptionService.Domain.Enums;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace OnyxDoc.SubscriptionService.Application.Common.Models
{
    public class CreatePGProductRequest
    {
        public int PaymentGatewayProductId { get; set; }
        public string PaymentGatewayProductCode { get; set; }
        public PaymentGateway PaymentGateway { get; set; }
    }
}