using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.Common.Models
{
    public class CreateContractTypeRoleInitiatorRequest
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
  

}
