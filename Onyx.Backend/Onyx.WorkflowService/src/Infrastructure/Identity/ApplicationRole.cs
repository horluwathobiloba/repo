using Microsoft.AspNetCore.Identity;
using Onyx.WorkFlowService.Domain.Entities;
using Onyx.WorkFlowService.Domain.Enums;
using System;

namespace Onyx.WorkFlowService.Infrastructure.Identity
{
    public class ApplicationRole : IdentityRole
    {
        public AccessLevel AccessLevel { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public decimal LoanTransactionLimit { get; set; }
        public int MaxLoanCountBooked { get; set; }
        public decimal MaxLoanVolumeBooked { get; set; }
        public string RoleId { get; set; }
    }
}
