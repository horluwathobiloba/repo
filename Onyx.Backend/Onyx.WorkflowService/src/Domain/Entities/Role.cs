using Onyx.WorkFlowService.Domain.Common;
using Onyx.WorkFlowService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.WorkFlowService.Domain.Entities
{
    public class Role : AuditableEntity
    {
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public decimal LoanTransactionLimit { get; set; }
        public int MaxLoanCountBooked { get; set; }
        public decimal MaxLoanVolumeBooked { get; set; }
        public string RoleId { get; set; }
        public RolePermission RolePermission { get; set; }
        public string RolePermissionId { get; set; }
    }
}
