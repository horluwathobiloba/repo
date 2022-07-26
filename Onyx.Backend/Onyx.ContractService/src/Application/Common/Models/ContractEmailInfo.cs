using Onyx.ContractService.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.Common.Models
{
    public class ContractEmailInfo
    {
        public Domain.Entities.Contract Contract { get; set; }
        public List<string>EmailRecipients { get; set; }
        public List<EmailRecipient> Emails { get; set; }
        public string ExpiryTime { get; set; }
        public int ExpiryMonthValue { get; set; }
        public int ExpiryYearValue { get; set; }
        public int DaysBetween { get; set; }
    }
}
