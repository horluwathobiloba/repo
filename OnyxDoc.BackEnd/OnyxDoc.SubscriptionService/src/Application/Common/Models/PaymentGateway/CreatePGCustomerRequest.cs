using OnyxDoc.SubscriptionService.Domain.Enums;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace OnyxDoc.SubscriptionService.Application.Common.Models
{
    public class CreatePGCustomerRequest
    {
        public int PaymentGatewayCustomerId{ get; set; }
        public string PaymentGatewayCustomerCode { get; set; }
        public PaymentGateway PaymentGateway { get; set; }
    }
}