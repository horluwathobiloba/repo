using OnyxDoc.DocumentService.Domain.Entities;
using OnyxDoc.DocumentService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.DocumentService.Domain.ViewModels
{
    public class DimensionVm
    {
        public CoordinateVm Coordinate { get; set; }
        public Type Type { get; set; }
        public string Name { get; set; }
        public string[] SelectOptions { get; set; }
        public string[] Validators { get; set; }
        public string Value { get; set; }
    }
}