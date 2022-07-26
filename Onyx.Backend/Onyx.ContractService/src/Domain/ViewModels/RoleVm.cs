using Onyx.ContractService.Domain.Common;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.ContractService.Domain.Entities
{
    public class RoleVm 
    {
        public bool Succeeded { get; set; }
        public RoleDto Entity { get; set; }
        public string[] Messages { get; set; }
    }
}
