using OnyxDoc.AuthService.Domain.Common;
using OnyxDoc.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.AuthService.Domain.Entities
{
   public class User : AuditableEntity
    {

        public string FirstName { get; set; }
        public ThirdPartyType? ThirdPartyType { get; set; }
        public string JobTitle { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public DateTime LastLoginDate { get; set; }
        public string UserId { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string ProfilePicture { get; set; }
        public string Token { get; set; }
        public bool EmailConfirmed { get; set; }
        public UserCreationStatus UserCreationStatus { get; set; }
        public string UserCreationStatusDesc { get; set; }
        public string Signature { get; set; }
    }
}
