using RubyReloaded.AuthService.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Domain.Entities
{ 
   public class CooperativeUserMapping:AuditableEntity
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public int CooperativeId { get; set; }
        public Cooperative Cooperative { get; set; }
        public CooperativeAccessStatus CooperativeAccessStatus { get; set; }
        public int RoleId { get; set; }
    }
}
  