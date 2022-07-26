using Onyx.AuthService.Domain.Common;
using Onyx.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.AuthService.Domain.Entities
{
   public class User : AuditableEntity
    {
        public string FirstName { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public Gender Gender { get; set; }
        public int OrganizationId { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public int JobFunctionId { get; set; }
        public JobFunction JobFunction { get; set; }
        public string UserCode { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime LastAccessedDate { get; set; }
        public DateTime EmploymentDate { get; set; }
        public string UserId { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string ProfilePicture { get; set; }
        public string Token { get; set; }
    }
}
