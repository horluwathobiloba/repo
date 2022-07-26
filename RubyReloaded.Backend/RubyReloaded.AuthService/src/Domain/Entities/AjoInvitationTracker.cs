using RubyReloaded.AuthService.Domain.Common;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Domain.Entities
{
    public class AjoInvitationTracker:AuditableEntity
    {
        public string UserEmail { get; set; }
        public string AdminEmail { get; set; }
        public int AjoId { get; set; }
        public RequestType RequestType { get; set; }
    }
}
