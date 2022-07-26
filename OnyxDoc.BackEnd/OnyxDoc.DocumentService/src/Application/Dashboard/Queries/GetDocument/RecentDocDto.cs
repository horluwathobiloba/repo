using OnyxDoc.DocumentService.Application.Common.Mappings;
using System;
using OnyxDoc.DocumentService.Domain.Entities;
using System.Collections.Generic;
using System.Text;
using OnyxDoc.DocumentService.Domain.Enums;

namespace OnyxDoc.DocumentService.Application.Dashboard.Queries.GetDocument
{
    public class RecentDocDto : IMapFrom<Domain.Entities.Document>
    {
        public string DocumentMessage { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public DocumentStatus DocumentStatus { get; set; }
        public string DocumentStatusDesc { get; set; }
    }
}
