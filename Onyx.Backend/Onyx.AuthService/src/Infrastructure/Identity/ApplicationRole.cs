using Microsoft.AspNetCore.Identity;
using Onyx.AuthService.Domain.Entities;
using Onyx.AuthService.Domain.Enums;
using System;

namespace Onyx.AuthService.Infrastructure.Identity
{
    public class ApplicationRole : IdentityRole
    {
        public AccessLevel AccessLevel { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
      
        public string RoleId { get; set; }
    }
}
