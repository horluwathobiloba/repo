using RubyReloaded.SubscriptionService.Domain.Common;
using RubyReloaded.SubscriptionService.Domain.Enums;
using RubyReloaded.SubscriptionService.Domain.ViewModels;

namespace RubyReloaded.SubscriptionService.Domain.ViewModels
{
    public class RoleDto : AuthEntity
    { 
        public UserAccessLevel UserAccessLevel { get; set; }
        public string UserAccessLevelDesc { get; set; }
        public WorkflowUserCategory WorkflowUserCategory { get; set; }
        public string WorkflowUserCategoryDesc { get; set; }
        public SubscriberDto Subscriber { get; set; }
    }
}
