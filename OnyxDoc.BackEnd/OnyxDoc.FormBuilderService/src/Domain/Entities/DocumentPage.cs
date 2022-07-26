using OnyxDoc.FormBuilderService.Domain.Common;
using OnyxDoc.FormBuilderService.Domain.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnyxDoc.FormBuilderService.Domain.Entities
{
    public class DocumentPage : AuditableEntity
    {
        public string DisplayName { get; set; }
        public string Watermark { get; set; }
        public string PageTips { get; set; }
        public int DocumentId { get; set; }
        public int PageIndex { get; set; }
        public int PageNumber { get; set; }
        public PageLayout PageLayout { get; set; }


        #region Page Dimension
        public string Height { get; set; }
        public string Width { get; set; }
        public string Position { get; set; }
        public string Transform { get; set; }
        #endregion

        public string HeaderData { get; set; }
        public string FooterData { get; set; }

        [ForeignKey(nameof(DocumentId))]
        public Document Document { get; set; }

        public List<PageControlItem> PageControlItems { get; set; }

    }
}
