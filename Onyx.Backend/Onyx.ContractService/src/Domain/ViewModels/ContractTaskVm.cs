using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Domain.ViewModels
{
    public class ContractTaskVm
    {
        public string TaskName { get; set; }
        public string AssignedToUserId { get; set; }

        public string AssignedToEmail { get; set; }

        public DateTime DueDate { get; set; }
    }
}
