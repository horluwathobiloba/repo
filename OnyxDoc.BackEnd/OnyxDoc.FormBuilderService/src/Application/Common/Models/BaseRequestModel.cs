using OnyxDoc.FormBuilderService.Domain.Enums;
using System;
using System.Text.Json.Serialization;

namespace OnyxDoc.FormBuilderService.Application.Common.Models
{
    public class BaseRequestModel : AuthToken
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }       
    }
}