using OnyxDoc.FormBuilderService.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnyxDoc.FormBuilderService.Domain.ViewModels
{
    public class APIResponseVm
    {
        public bool succeeded { get; set; }
        public object entity { get; set; }
        public object permissions { get; set; }
        public string[] messages { get; set; }
        public string message { get; set; }
    }
}