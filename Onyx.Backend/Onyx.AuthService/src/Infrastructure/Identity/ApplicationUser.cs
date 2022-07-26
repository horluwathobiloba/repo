﻿using Microsoft.AspNetCore.Identity;
using Onyx.AuthService.Domain.Entities;
using Onyx.AuthService.Domain.Enums;
using System;

namespace Onyx.AuthService.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Address { get; set; }
        public Gender Gender { get; set; }

        public int OrganizationId { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public string UserCode { get; set; }
        public int DepartmentId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime LastAccessedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string  CreatedById { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string UserId { get; set; }
        public DateTime EmploymentDate { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string CurrentLocationAddress { get; set; }
        public string ProfilePicture { get; set; }
        public string Token { get; set; }
        public int? JobFunctionId { get; set; }
       public JobFunction JobFunction { get; set; }
    }
}
