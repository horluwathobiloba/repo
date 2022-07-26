using OnyxDoc.FormBuilderService.Domain.Enums; 

namespace OnyxDoc.FormBuilderService.Application.Common.Models
{
    public class CreateControlPropertyItemRequest : BaseRequestModel
    { 
        public int ControlId { get; set; }  
        public int ParentPropertyId { get; set; }
        public int ControlPropertyId { get; set; }
        public int Index { get; set; }
        public string Value { get; set; }
        public bool IsDefaultValue { get; set; } 
    }
}