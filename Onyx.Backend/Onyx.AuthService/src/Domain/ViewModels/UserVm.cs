using Onyx.AuthService.Domain.Entities;
using Onyx.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.AuthService.Domain.ViewModels
{
   public class UserVm
    {
        public string FirstName { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
