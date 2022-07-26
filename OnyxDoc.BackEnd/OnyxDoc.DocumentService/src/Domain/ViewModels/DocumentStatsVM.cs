using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.DocumentService.Domain.ViewModels
{
    public class DocumentStatsVM
    {
        public string Form { get; set; }
        public int Document { get; set; }
        public string Month { get; set; }
        public string Process { get; set; }
    }
}
