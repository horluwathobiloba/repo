using RubyReloaded.AuthService.Domain.Common;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Domain.Entities
{
   public class User : AuditableEntity
    {
        public string FirstName { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string BVN { get; set; }
        //public int RoleId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime LastAccessedDate { get; set; }
        public string UserId { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string ProfilePicture { get; set; }
        public string Token { get; set; }
        public UserAccessLevel UserAccessLevel { get; set; }
        public string TransactionPin { get; set; }  
        public Gender Gender { get; set; }
        public string UserName { get; set; }
        public bool NotificationStatus { get; set; }
    }
}
