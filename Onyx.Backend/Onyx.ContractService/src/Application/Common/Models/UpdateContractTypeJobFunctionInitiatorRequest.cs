using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.Common.Models
{
    public class UpdateContractTypeJobFunctionInitiatorRequest
    {
        public int ContractInitiatorId { get; set; }
        public int InitiatorJobFunctionId { get; internal set; }
        public string InitiatorJobFunctionName { get; internal set; }
    }
}
