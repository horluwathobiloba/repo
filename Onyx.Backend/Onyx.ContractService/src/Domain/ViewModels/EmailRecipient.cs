using Onyx.ContractService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Domain.ViewModels
{
    public class EmailRecipient
    {
        public Contract Contract { get; set; }
        public string RecipientEmail { get; set; }
        public string ExpiryTime { get; set; }
        public int ExpiryMonthValue { get; set; }
        public int ExpiryYearValue { get; set; }
        public int DaysBetween { get; set; }
    }
}
