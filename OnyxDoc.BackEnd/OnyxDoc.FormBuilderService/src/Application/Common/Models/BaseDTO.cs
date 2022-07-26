using OnyxDoc.FormBuilderService.Domain.Enums;
using System;
using System.Text.Json.Serialization;

namespace OnyxDoc.FormBuilderService.Application.Common.Models
{
    public class BaseDTO
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }
    }
}