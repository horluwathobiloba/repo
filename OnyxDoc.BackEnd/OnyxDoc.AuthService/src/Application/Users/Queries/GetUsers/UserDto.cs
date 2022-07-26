using AutoMapper;
using System;
using OnyxDoc.AuthService.Domain.Entities;
using OnyxDoc.AuthService.Application.Common.Mappings;
using OnyxDoc.AuthService.Domain.Enums;
using OnyxDoc.AuthService.Domain.Common;

namespace OnyxDoc.AuthService.Application.Users.Queries.GetUsers
{
    public class UserDto : IMapFrom<User>
    {
        public string UserId { get; set; }
        public int SubscriberId { get; set; }
        public Subscriber Subscriber { get; set; }
        public bool EmailConfirmed { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string UserCode { get; set; }
        public string JobTitle { get; set; }
        public string Signature { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime EmploymentDate { get; set; }
        public string CreatedById { get; set; }
        public string CreatedByEmail { get; set; }
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
        public string ProfilePicture { get; set; }
        public string StatusDesc { get; set; }
        public UserCreationStatus UserCreationStatus { get; set; }
        public string UserCreationStatusDesc { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, UserDto>();
            profile.CreateMap<UserDto, User>();
        }
    }
}
