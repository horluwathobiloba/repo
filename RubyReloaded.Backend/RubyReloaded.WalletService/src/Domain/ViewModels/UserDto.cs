using System;

namespace RubyReloaded.WalletService.Domain.ViewModels
{
    public class UserDto : UserAuthEntity
    {
        public UserDto()
        {

        }



        public int RoleId { get; set; }
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

    public class SubscriberAdminDto
    {
        public string userId { get; set; }
        public int roleId { get; set; }
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string lastName { get; set; }
        public string phoneNumber { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string userCode { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string profilePicture { get; set; }
    }
}
