using OnyxDoc.FormBuilderService.Domain.Common;
using OnyxDoc.FormBuilderService.Domain.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnyxDoc.FormBuilderService.Domain.Entities
{
    public class Control : AuditableEntity
    {
        public string DisplayName { get; set; }
        public string ControlTips { get; set; }
        public int InitialControlVersionId { get; set; }
        public decimal VersionNumber { get; set; }
        public InputValueType InputValueType { get; set; }
        public string InputValueTypeDesc { get; set; }
        public ControlType ControlType { get; set; }
        public string ControlTypeDesc { get; set; }
        public BlockControlType? BlockControlType { get; set; }
        public string BlockControlTypeDesc { get; set; }
        public FieldControlType? FieldControlType { get; set; }
        public string FieldControlTypeDesc { get; set; }

        public ICollection<ControlProperty> ControlProperties { get; set; }
    }
}
