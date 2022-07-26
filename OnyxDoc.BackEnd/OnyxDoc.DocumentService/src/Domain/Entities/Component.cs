using Newtonsoft.Json;
using OnyxDoc.DocumentService.Domain.Common;
using OnyxDoc.DocumentService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.DocumentService.Domain.Entities
{
    public class Component : AuditableEntity
    {
        public int RecipientId { get; set; }
        public Coordinate Coordinate { get; set; }
        public LabelType Type { get; set; }
        public string Label { get; set; }
        public int Rank { get; set; }
        public string SelectOptions { get; set; }
        public string Validators { get; set; }
        public string Value { get; set; }
        public int DocumentId { get; set; }
        public string FilePath { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public string Hash { get; set; }
        public string PageNumber { get; set; }
        public string UserId { get; set; }
    }
}
