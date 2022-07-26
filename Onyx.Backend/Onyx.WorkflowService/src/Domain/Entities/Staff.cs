using Onyx.WorkFlowService.Domain.Common;
using Onyx.WorkFlowService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.WorkFlowService.Domain.Entities
{
   public class Staff : AuditableEntity
    {
        public string FirstName { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public Gender Gender { get; set; }
        public int MarketId { get; set; }
        public int OrganizationId { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public string StaffCode { get; set; }
        public int DepartmentId { get; set; }
        public string SupervisorId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime LastAccessedDate { get; set; }
        public DateTime EmploymentDate { get; set; }
        public string StaffId { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string CurrentLocationAddress { get; set; }
    }
}
