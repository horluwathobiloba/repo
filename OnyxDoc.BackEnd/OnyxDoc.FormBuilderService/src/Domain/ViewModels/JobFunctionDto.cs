using OnyxDoc.FormBuilderService.Domain.Common;
using OnyxDoc.FormBuilderService.Domain.Enums;
using OnyxDoc.FormBuilderService.Domain.ViewModels;
using OnyxDoc.FormBuilderService.Domain.ViewModels.Common;

namespace OnyxDoc.FormBuilderService.Domain.ViewModels
{
    public class JobFunctionDto : AuthEntity
    { 
        public SubscriberDto Subscriber { get; set; }
    }
}
