

using OnyxDoc.DocumentService.Domain.Common;

namespace OnyxDoc.DocumentService.Domain.ViewModels
{
    public class JobFunctionDto : AuthEntity
    { 
        public SubscriberDto Subscriber { get; set; }
    }
}
