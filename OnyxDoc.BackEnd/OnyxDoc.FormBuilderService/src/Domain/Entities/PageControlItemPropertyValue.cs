using OnyxDoc.FormBuilderService.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnyxDoc.FormBuilderService.Domain.Entities
{
    public class PageControlItemPropertyValue : AuditableEntity
    {
        public int PageControlItemPropertyId { get; set; }
        public string Value { get; set; }

        [ForeignKey(nameof(PageControlItemPropertyId))]
        public PageControlItemProperty PageControlItemProperty { get; set; }
    }
}
