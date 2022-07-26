using OnyxDoc.SubscriptionService.Domain.Common;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnyxDoc.SubscriptionService.Domain.Entities
{
    public class SubscriptionPlanPricing : AuditableEntity
    {
        public int SubscriptionPlanId { get; set; }
        public string StripePriceId { get; set; }
        public string CurrencyCode { get; set; } 
        public int CurrencyId { get; set; }
        public PricingPlanType PricingPlanType { get; set; }
        public string PricingPlanTypeDesc { get; set; }        
        public decimal Amount { get; set; }
        public decimal Discount { get; set; } 
        [ForeignKey(nameof(SubscriptionPlanId))]
        public SubscriptionPlan  SubscriptionPlan { get; set; }

        [ForeignKey(nameof(CurrencyId))]
        public Currency Currency { get; set; }
    }
}
