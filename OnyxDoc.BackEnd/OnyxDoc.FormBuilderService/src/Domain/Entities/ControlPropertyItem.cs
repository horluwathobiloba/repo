using OnyxDoc.FormBuilderService.Domain.Common;
using OnyxDoc.FormBuilderService.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnyxDoc.FormBuilderService.Domain.Entities
{
    public class ControlPropertyItem : AuditableEntity
    {
        public int ControlPropertyId { get; set; }
        public int Index { get; set; }
        public string Value { get; set; }
        public bool IsDefaultValue { get; set; }

        [ForeignKey(nameof(ControlPropertyId))]
        public ControlProperty ControlProperty { get; set; }       
    }
}
