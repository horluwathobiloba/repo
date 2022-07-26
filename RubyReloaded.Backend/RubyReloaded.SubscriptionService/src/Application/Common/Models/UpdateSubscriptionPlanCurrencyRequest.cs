using RubyReloaded.SubscriptionService.Domain.Enums;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace RubyReloaded.SubscriptionService.Application.Common.Models
{
    public class UpdateSubscriptionPlanPricingRequest
    {
        public int Id { get; set; }
        public int SubscriptionPlanId { get; set; }
        public string CurrencyCode { get; set; }
        public int CurrencyId { get; set; }

        public bool EnableMonthlyPricingPlan { get; set; }
        public decimal MonthlyAmount { get; set; }
        public decimal MonthlyDiscount { get; set; }
        public bool EnableYearlyPricingPlan { get; set; }
        public decimal YearlyAmount { get; set; }
        public decimal YearlyDiscount { get; set; }
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