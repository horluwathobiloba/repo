using OnyxDoc.DocumentService.Domain.Common;
using OnyxDoc.DocumentService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.DocumentService.Domain.Entities
{
    public class Coordinate : AuditableEntity
    {
        public string Position { get; set; }
        public string Transform { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public CoordinateType CoordinateType { get; set; }
    }
}
