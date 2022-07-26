using OnyxDoc.FormBuilderService.Domain.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnyxDoc.FormBuilderService.Domain.Entities
{
    public class PageControlItemProperty: AuditableEntity
    {

        public int PageControlItemId { get; set; }
        public int ControlPropertyId { get; set; } 

        [ForeignKey(nameof(PageControlItemId))]
        public PageControlItem PageControlItem { get; set; }

        [ForeignKey(nameof(ControlPropertyId))]
        public ControlProperty ControlProperty { get; set; }

        public List<PageControlItemPropertyValue> PageControlItemPropertyValues { get; set; }
    }
}
