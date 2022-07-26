using OnyxDoc.FormBuilderService.Domain.Enums;
using System.Collections.Generic;

namespace OnyxDoc.FormBuilderService.Application.Common.Models
{
    public class UpdateDocumentPageDimensionRequest : BaseRequestModel
    {
        public int ControlId { get; set; }
        public int DocumentId { get; set; }

        #region Page Dimension
        public string Position { get; set; }
        public string Transform { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
        #endregion
        public bool IsDeleted { get; set; }

        public List<UpdatePageControlItemRequest> PageControlItems { get; set; }
        public Status Status
        {
            get
            {
                return IsDeleted ? Status.Deactivated : Status.Active;
            }
        }
    }
}