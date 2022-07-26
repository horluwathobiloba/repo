using OnyxDoc.FormBuilderService.Domain.Enums;

namespace OnyxDoc.FormBuilderService.Application.Common.Models
{
    public class UpdatePageControlItemPropertyValueRequest : BaseRequestModel
    { 
        public string Value { get; set; }

        public bool IsDeleted { get; set; }

        public Status Status
        {
            get
            {
                return IsDeleted ? Status.Deactivated : Status.Active;
            }
        }
    }
}