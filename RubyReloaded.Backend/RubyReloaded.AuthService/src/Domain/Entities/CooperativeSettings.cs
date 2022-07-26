using RubyReloaded.AuthService.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Domain.Entities
{
    public class CooperativeSettings:AuditableEntity
    {
        public int CooperativeId { get; set; }
        public bool RequestToJoin { get; set; }
        public bool Visible { get; set; }
        public string AdminEmail { get; set; }
    }
}
