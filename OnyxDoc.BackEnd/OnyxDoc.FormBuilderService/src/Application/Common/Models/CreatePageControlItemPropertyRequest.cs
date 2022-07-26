using System.Collections.Generic;

namespace OnyxDoc.FormBuilderService.Application.Common.Models
{
    public class CreatePageControlItemPropertyRequest : BaseRequestModel
    {
        public int ControlPropertyId { get; set; }
        public List<CreatePageControlItemPropertyValueRequest> PageControlItemPropertyValues { get; set; }       
    }
}