using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.Common.Models
{
    public class UpdateContractTypeInitiatorRequest
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int JobFunctionId { get; internal set; }
        public string JobFunctionName { get; internal set; }
    }
}
