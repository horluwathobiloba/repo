using OnyxDoc.FormBuilderService.Domain.Enums;
using System.Collections.Generic;

namespace OnyxDoc.FormBuilderService.Application.Common.Models
{
    public class CreateDocumentPageRequest : BaseRequestModel
    {
        public int ControlId { get; set; }
        public int ParentPropertyId { get; set; }
        public string DisplayName { get; set; }
        public string PageTips { get; set; }
        public int DocumentId { get; set; }
        public int PageIndex { get; set; }
        public int PageNumber { get; set; }
        public string Watermark { get; set; }
        public PageLayout PageLayout { get; set; }

        #region Page Dimension
        public string Position { get; set; }
        public string Transform { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
        #endregion

        public string HeaderData { get; set; }
        public string FooterData { get; set; }

        public bool IsDeleted { get; set; } 

        public List<UpdatePageControlItemRequest> PageControlItems { get; set; }
    }
}