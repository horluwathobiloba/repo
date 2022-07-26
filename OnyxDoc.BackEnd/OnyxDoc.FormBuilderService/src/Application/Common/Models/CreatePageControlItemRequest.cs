using OnyxDoc.FormBuilderService.Domain.Enums;
using System.Collections.Generic;

namespace OnyxDoc.FormBuilderService.Application.Common.Models
{
    public class CreatePageControlItemRequest : BaseRequestModel
    { 
        public int ControlId { get; set; }

        #region Page ControlItem Dimension
        public string Height { get; set; }
        public string Width { get; set; }
        public string Position { get; set; }
        public string Transform { get; set; }
        #endregion
        public List<UpdatePageControlItemPropertyRequest> PageControlItemProperties { get; set; } 
    }
}