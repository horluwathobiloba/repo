using Microsoft.AspNetCore.Identity;
using OnyxDoc.AuthService.Domain.Entities;
using OnyxDoc.AuthService.Domain.Enums;
using System;

namespace OnyxDoc.AuthService.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string JobTitle { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; }
        public int SubscriberId { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public DateTime LastLoginDate { get; set; }
        public string UserId { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string ProfilePicture { get; set; }
        public string CreatedByEmail { get; set; }
        public DateTime CreatedDate { get; set; }
        public string  CreatedById { get; set; }
        public string LastModifiedById { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string LastModifiedByEmail { get; set; }
        public Status Status { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Token { get; set; }
        public string Signature { get; set; }
        public UserCreationStatus UserCreationStatus { get; set; }
        public string UserCreationStatusDesc { get; set; }
    }
}
