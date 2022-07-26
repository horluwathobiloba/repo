using OnyxDoc.FormBuilderService.Domain.Enums;
using System.Collections.Generic;

namespace OnyxDoc.FormBuilderService.Application.Common.Models
{
    public class UpdatePageControlItemPropertyRequest : BaseRequestModel
    {
        public int ControlPropertyId { get; set; }
        public List<UpdatePageControlItemPropertyValueRequest> PageControlItemPropertyValues { get; set; }

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