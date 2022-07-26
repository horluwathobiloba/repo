using OnyxDoc.FormBuilderService.Domain.Enums;
using System.Collections.Generic;

namespace OnyxDoc.FormBuilderService.Application.Common.Models
{
    public class CreateControlPropertyRequest : BaseRequestModel
    {
        public int ParentPropertyId { get; set; }
        public int Index { get; set; }
        public string DisplayName { get; set; }
        public string PropertyTips { get; set; }
        public ControlPropertyType ControlPropertyType { get; set; }
        public ControlPropertyValueType ControlPropertyValueType { get; set; }
        public bool ShowInContextMenu { get; set; }
        public List<UpdateControlPropertyItemRequest> ControlPropertyItems { get; set; }
    }
}