using OnyxDoc.DocumentService.Domain.Entities;
using OnyxDoc.DocumentService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.DocumentService.Domain.ViewModels
{
    public class ComponentVm
    {
        public int? Id { get; set; }
        public int RecipientId { get; set; }
        public CoordinateVm Coordinate { get; set; }
        public string RecipientEmail { get; set; }
        public string Label { get; set; }
        public LabelType Type { get; set; }
        public string Name { get; set; }
        public string[] SelectOptions { get; set; }
        public string[] Validators { get; set; }
        public string Value { get; set; }
        public int Rank { get; set; }
        public string PageNumber { get; set; }
    }
}