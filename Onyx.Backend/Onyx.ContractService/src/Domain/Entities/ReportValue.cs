using Onyx.ContractService.Domain.Common;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Domain.Entities
{
    public class ReportValue:AuditableEntity
    {
        public string? ContractReportName  { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ContractStatus? ContractStatus { get; set; }
        public int? ProductTypeId { get; set; }
        public int? JobFunctionId { get; set; }
        public int? ContractTypeId { get; set; }
        public DocumentType ModuleName { get; set; }
    }
}
