using RubyReloaded.AuthService.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Domain.Entities
{
    public class UserLinkInvite:AuditableEntity
    {
        public string UserId { get; set; }
        public string RecipientEmail { get; set; }
        public bool IsUsed { get; set; }
        public string CooperativeId { get; set; }
        public string AjoId { get; set; }
        public string Code { get; set; }
        public string Token { get; set; }
    }
}
