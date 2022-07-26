using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.Common.Models
{
  
    public class CreateContractTypeJobFunctionInitiatorRequest
    {
        public int Id { get; set; }
        public int JobFunctionId { get; set; }
        public string JobFunctionName { get; set; }
    }

}
