using RubyReloaded.AuthService.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Domain.Entities
{
    public class UserCount : AuditableEntity
    {
        public int Count { get; set; }
        public int OrganizationId { get; set; }
    }
}
