using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.DocumentService.Domain.ViewModels
{
    public class ChartVm
    {
        public string Month { get; set; }
        public int Sent { get; set; }
        public int Awaiting { get; set; }
        public int Completed { get; set; }
        public int Expired { get; set; }
    }
}
