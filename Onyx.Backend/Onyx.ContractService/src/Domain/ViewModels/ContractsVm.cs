using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Domain.ViewModels
{
    public class ContractsVm
    {
        public string VendorName { get; set; }

        public DocumentType DocumentType { get; set; }
        public string DocumentTypeDesc { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ExpirationDate { get; set; }

        public int TimeLeft { get; set; }
    }
}
