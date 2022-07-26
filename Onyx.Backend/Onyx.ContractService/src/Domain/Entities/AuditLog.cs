using Onyx.ContractService.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Domain.Entities
{
    public class AuditLog: AuditableEntity
    {
        public string UserId { get; set; }
        public string Module { get; set; }
        public string Action { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public int JobFunctionId { get; set; }
        public string JobFunctionName { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
