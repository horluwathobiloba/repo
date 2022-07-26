using OnyxDoc.SubscriptionService.Domain.Enums;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace OnyxDoc.SubscriptionService.Application.Common.Models
{
    public class UpdateCurrencyRequest
    {
        public int Id { get; set; }
        public CurrencyCode CurrencyCode { get; set; } 
        [DefaultValue(false)]
        public bool IsDefault { get; set; }
    }
}