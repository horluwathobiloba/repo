using RubyReloaded.SubscriptionService.Domain.Enums;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace RubyReloaded.SubscriptionService.Application.Common.Models
{
    public class CreateSubscriptionPlanFeatureRequest
    {  
        public int FeatureId { get; set; }
        public string FeatureName { get; set; }
        public int ParentFeatureId { get; set; }
        public int ParentFeatureName { get; set; }
    }
}