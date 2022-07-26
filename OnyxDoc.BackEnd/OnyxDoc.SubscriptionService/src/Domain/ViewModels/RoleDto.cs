using OnyxDoc.SubscriptionService.Domain.Common;
using OnyxDoc.SubscriptionService.Domain.Enums;
using OnyxDoc.SubscriptionService.Domain.ViewModels;
using OnyxDoc.SubscriptionService.Domain.ViewModels.Common;

namespace OnyxDoc.SubscriptionService.Domain.ViewModels
{
    public class RoleDto : AuthEntity
    { 
        public RoleAccessLevel RoleAccessLevel { get; set; }
        public string RoleAccessLevelDesc { get; set; }
        public WorkflowUserCategory WorkflowUserCategory { get; set; }
        public string WorkflowUserCategoryDesc { get; set; }
        public SubscriberDto Subscriber { get; set; }
    }

    public class RoleObjectDto
    {
        public string name { get; set; }
        public RoleAccessLevel roleAccessLevel { get; set; }
        public string roleAccessLevelDesc { get; set; }
        public WorkflowUserCategory workflowUserCategory { get; set; }
        public string workflowUserCategoryDesc { get; set; }
    }
}
