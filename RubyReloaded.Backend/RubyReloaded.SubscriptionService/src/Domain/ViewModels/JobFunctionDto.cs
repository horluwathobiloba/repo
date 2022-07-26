using RubyReloaded.SubscriptionService.Domain.Common;
using RubyReloaded.SubscriptionService.Domain.Enums;
using RubyReloaded.SubscriptionService.Domain.ViewModels;

namespace RubyReloaded.SubscriptionService.Domain.ViewModels
{
    public class JobFunctionDto : AuthEntity
    { 
        public SubscriberDto Subscriber { get; set; }
    }
}
