using OnyxDoc.SubscriptionService.Domain.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OnyxDoc.SubscriptionService.Application.Common.Models
{
    public class UpdateSubscriptionPlanFeatureRequest
    {
        public int Id { get; set; }
        public int FeatureId { get; set; }
        public string FeatureName { get; set; }
        public int ParentFeatureId { get; set; }
        public string ParentFeatureName { get; set; }
        public bool IsChecked { get; set; }

        [NotMapped]
        public  Status Status
        {
            get
            {
                return IsChecked ? Status.Active : Status.Deactivated;
            }
        }
    }
}