using OnyxDoc.FormBuilderService.Domain.Common;
using OnyxDoc.FormBuilderService.Domain.Enums;
using OnyxDoc.FormBuilderService.Domain.ViewModels;
using OnyxDoc.FormBuilderService.Domain.ViewModels.Common;

namespace OnyxDoc.FormBuilderService.Domain.ViewModels
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
