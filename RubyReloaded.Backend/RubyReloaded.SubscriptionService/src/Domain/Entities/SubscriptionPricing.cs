using RubyReloaded.SubscriptionService.Domain.Common;
using RubyReloaded.SubscriptionService.Domain.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace RubyReloaded.SubscriptionService.Domain.Entities
{
    public class SubscriptionPlanPricing : AuditableEntity
    {
        public int SubscriptionPlanId { get; set; }      
        public string CurrencyCode { get; set; } 
        public int CurrencyId { get; set; }

        public bool EnableMonthlyPricingPlan { get; set; }
        public decimal MonthlyAmount { get; set; }
        public decimal MonthlyDiscount { get; set; }
        public bool EnableYearlyPricingPlan { get; set; }
        public decimal YearlyAmount { get; set; }
        public decimal YearlyDiscount { get; set; }

        [ForeignKey(nameof(SubscriptionPlanId))]
        public SubscriptionPlan  SubscriptionPlan { get; set; }

        [ForeignKey(nameof(CurrencyId))]
        public Currency Currency { get; set; }
    }
}
