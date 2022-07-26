using Newtonsoft.Json;
using Onyx.ContractService.Domain.Common;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.ContractService.Domain.Entities
{
    public class UserDto : UserAuthEntity
    {
        public string UserId { get; set; }
        public int OrganizationId { get; set; }
        public OrganisationDto Organization { get; set; }
        public int RoleId { get; set; }
        public RoleDto Role { get; set; }
        public int JobFunctionId { get; set; }
        public JobFunctionDto JobFunction { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public Gender Gender { get; set; }
        public string UserCode { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime EmploymentDate { get; set; }
        public DateTime LastAccessedDate { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string CurrentLocationAddress { get; set; }
        public string ProfilePicture { get; set; }
        
    }
}
