using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.Common.Models
{
    public class CreateContractTypeInitiatorRequest
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public int JobFunctionId { get; set; }
        public string JobFunctionName { get; set; }
    }
  

}
