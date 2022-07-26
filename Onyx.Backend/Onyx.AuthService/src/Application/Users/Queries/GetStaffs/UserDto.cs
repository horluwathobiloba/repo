using AutoMapper;
using System;
using Onyx.AuthService.Domain.Entities;
using Onyx.AuthService.Application.Common.Mappings;
using Onyx.AuthService.Domain.Enums;
using Onyx.AuthService.Domain.Common;
using Onyx.AuthService.Application.JobFunctions.Queries.GetJobFunctions;

namespace Onyx.AuthService.Application.Users.Queries.GetUsers
{
    public class UserDto : IMapFrom<User>
    {
        public string UserId { get; set; }
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
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
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public DateTime LastAccessedDate { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string CurrentLocationAddress { get; set; }
        public int JobFunctionId { get; set; }
        public JobFunction JobFunction { get; set; }
        public string ProfilePicture { get; set; }
        public string StatusDesc { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, UserDto>();
            profile.CreateMap<UserDto, User>();
        }
    }
}
