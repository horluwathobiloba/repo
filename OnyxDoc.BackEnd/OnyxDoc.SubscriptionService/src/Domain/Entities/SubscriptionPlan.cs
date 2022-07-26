using OnyxDoc.SubscriptionService.Domain.Common;
using OnyxDoc.SubscriptionService.Domain.Enums;
using OnyxDoc.SubscriptionService.Domain.ViewModels;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnyxDoc.SubscriptionService.Domain.Entities
{
    public class SubscriptionPlan : AuditableEntity
    {
        public string StripeProductId { get; set; }

        [DefaultValue(0)]
        public int NumberOfUsers { get; set; }

        [DefaultValue(0)]
        public int NumberOfTemplates { get; set; }

        [DefaultValue(0)]
        public int StorageSize { get; set; }
        public StorageSizeType StorageSizeType { get; set; }
        public string StorageSizeTypeDesc { get; set; }

        public bool AllowMonthlyPricing { get; set; }
        public bool AllowYearlyPricing { get; set; }

        [DefaultValue(false)]
        public bool AllowFreeTrial { get; set; }
        public int FreeTrialDays { get; set; } 
        public string Period { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
        public string SubscriptionTypeDesc { get; set; }
        public bool IsHighlighted { get; set; }

        [DefaultValue(false)]
        public bool AllowDiscount { get; set; }

        [DefaultValue(false)]
        public bool ShowSubscribeButton { get; set; }

        [DefaultValue(false)]
        public bool ShowContactUsButton { get; set; }
 
        public ICollection<SubscriptionPlanPricing> SubscriptionPlanPricings { get; set; }
        public ICollection<SubscriptionPlanFeature> SubscriptionPlanFeatures { get; set; }

        // [NotMapped]
        // public ICollection<FeatureDto> Features { get; set; }
    }
}
