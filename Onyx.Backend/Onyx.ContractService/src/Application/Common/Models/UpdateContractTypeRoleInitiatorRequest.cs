using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.Common.Models
{
    public class UpdateContractTypeRoleInitiatorRequest
    {
        public int InitiatorRoleId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
