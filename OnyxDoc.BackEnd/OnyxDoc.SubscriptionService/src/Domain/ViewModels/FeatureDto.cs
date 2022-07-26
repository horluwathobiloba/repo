using OnyxDoc.SubscriptionService.Domain.Common;
using OnyxDoc.SubscriptionService.Domain.Enums;
using OnyxDoc.SubscriptionService.Domain.ViewModels;
using OnyxDoc.SubscriptionService.Domain.ViewModels.Common;

namespace OnyxDoc.SubscriptionService.Domain.ViewModels
{
    public class FeatureDto : AuthEntity
    { 
        public int ParentFeatureId { get; set; }
        public int ParentFeatureName { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public string AccessLevelDesc { get; set; }
        public WorkflowUserCategory WorkflowUserCategory { get; set; }
        public string WorkflowUserCategoryDesc { get; set; }
        public SubscriberDto Subscriber { get; set; }
    }
}
