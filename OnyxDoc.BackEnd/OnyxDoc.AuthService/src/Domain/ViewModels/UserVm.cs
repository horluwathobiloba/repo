﻿using OnyxDoc.AuthService.Domain.Entities;
using OnyxDoc.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.AuthService.Domain.ViewModels
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
