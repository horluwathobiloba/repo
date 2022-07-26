using Onyx.ContractService.Domain.Common;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.ContractService.Domain.Entities
{
    public class UserVm 
    {
        public bool Succeeded { get; set; }
        public UserDto Entity { get; set; }
        public string[] Messages { get; set; }
        public string Message { get; set; }
    }
}
