using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.Common.Models
{
   public class ContractTaskRequest
    {
        public DateTime DueDate { get; set; }
        public string AssignedUserId { get; set; }
        public string AssignedUserEmail { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
    }
}
