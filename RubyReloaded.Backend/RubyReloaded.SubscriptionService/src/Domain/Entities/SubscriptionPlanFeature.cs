using RubyReloaded.SubscriptionService.Domain.Common;
using RubyReloaded.SubscriptionService.Domain.Enums;
using RubyReloaded.SubscriptionService.Domain.ViewModels;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace RubyReloaded.SubscriptionService.Domain.Entities
{
    public class SubscriptionPlanFeature : AuditableEntity
    {
        public int SubscriptionPlanId { get; set; }
        public int FeatureId { get; set; }
        public string FeatureName { get; set; }
        public int ParentFeatureId { get; set; }
        public int ParentFeatureName { get; set; } 

        public SubscriptionPlan SuscriptionPlan { get; set; }
        public FeatureDto Feature { get; set; }
    }
}
