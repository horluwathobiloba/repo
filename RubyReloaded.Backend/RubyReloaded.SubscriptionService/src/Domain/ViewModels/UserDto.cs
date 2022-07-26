using System;
using RubyReloaded.SubscriptionService.Domain.Common;
using RubyReloaded.SubscriptionService.Domain.ViewModels;

namespace RubyReloaded.SubscriptionService.Domain.ViewModels
{
    public class UserDto : UserAuthEntity
    {
        public UserDto()
        {

        }

        public SubscriberDto Subscriber { get; set; }
        public int RoleId { get; set; }
        public RoleDto Role { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string UserCode { get; set; }
        public DateTime LastAccessedDate { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string ProfilePicture { get; set; }
    }
}
