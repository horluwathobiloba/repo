using OnyxDoc.FormBuilderService.Domain.Enums;
using System.Collections.Generic;

namespace OnyxDoc.FormBuilderService.Application.Common.Models
{
    public class UpdatePageControlItemRequest : BaseRequestModel
    {
        public int DocumentPageId { get; set; }
        public int ControlId { get; set; }

        #region Page ControlItem Dimension
        public string Height { get; set; }
        public string Width { get; set; }
        public string Position { get; set; }
        public string Transform { get; set; }
        #endregion
        public bool IsDeleted { get; set; }
        public Status Status
        {
            get
            {
                return IsDeleted ? Status.Deactivated : Status.Active;
            }
        }
        public List<UpdatePageControlItemPropertyRequest> PageControlItemProperties { get; set; }

    }
}