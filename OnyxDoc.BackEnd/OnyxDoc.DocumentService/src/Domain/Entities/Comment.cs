using OnyxDoc.DocumentService.Domain.Common;
using OnyxDoc.DocumentService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OnyxDoc.DocumentService.Domain.Entities
{
    public class Comment : AuditableEntity
    {
        public int DocumentId { get; set; }
        public string Text { get; set; }
        public CommentType CommentType { get; set; }
        public Document Document { get; set; }
        public Coordinate Coordinate { get; set; }
    }
}
