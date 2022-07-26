using OnyxDoc.FormBuilderService.Domain.Common;
using OnyxDoc.FormBuilderService.Domain.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnyxDoc.FormBuilderService.Domain.Entities
{
    public class ControlProperty : AuditableEntity
    {
        //FK_ControlProperties_ControlProperties_ParentPropertyId
        public int ControlId { get; set; }
        public int? ParentPropertyId { get; set; }
        public int Index { get; set; }
        public string DisplayName { get; set; }
        public string PropertyTips { get; set; }
        public ControlPropertyType ControlPropertyType { get; set; }
        public string ControlPropertyTypeDesc { get; set; }

        public ControlPropertyValueType ControlPropertyValueType { get; set; }
        public string ControlPropertyValueTypeDesc { get; set; }
        public bool ShowInContextMenu { get; set; }

        [ForeignKey(nameof(ControlId))]
        public Control Control { get; set; }

        //[ForeignKey(nameof(ParentPropertyId))]
        //public ControlProperty ParentControlProperty { get; set; }
        /*public List<ControlPropertyItem> ControlPropertyItems { get; set; }*/
        public ICollection<ControlPropertyItem> ControlPropertyItems { get; set; }

    }
}
