using RubyReloaded.AuthService.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Domain.Entities
{
    public class CooperativeUserCode:AuditableEntity
    {
        public int CooperativeId { get; set; }
        public string Code { get; set; }

        public string Email { get; set; }
        public bool IsUsed { get; set; }
    }
}
