using OnyxDoc.SubscriptionService.Domain.Enums;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace OnyxDoc.SubscriptionService.Application.Common.Models
{
    public class UpdateSubscriptionPlanPricingRequest
    {
        public int Id { get; set; }
        public int SubscriptionPlanId { get; set; }
        public string CurrencyCode { get; set; }
        public int CurrencyId { get; set; }
        public PricingPlanType PricingPlanType { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; } 
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