using OnyxDoc.SubscriptionService.Domain.Common;
using OnyxDoc.SubscriptionService.Domain.Enums;
using OnyxDoc.SubscriptionService.Domain.ViewModels;
using OnyxDoc.SubscriptionService.Domain.ViewModels.Common;

namespace OnyxDoc.SubscriptionService.Domain.ViewModels
{
    public class JobFunctionDto : AuthEntity
    { 
        public SubscriberDto Subscriber { get; set; }
    }
}
