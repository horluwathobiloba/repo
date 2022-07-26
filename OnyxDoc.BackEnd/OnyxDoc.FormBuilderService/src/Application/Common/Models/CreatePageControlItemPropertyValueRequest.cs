using OnyxDoc.FormBuilderService.Domain.Enums;
using System.Collections.Generic;

namespace OnyxDoc.FormBuilderService.Application.Common.Models
{
    public class CreatePageControlItemPropertyValueRequest : BaseRequestModel
    { 
        public string Value { get; set; }

    }
}