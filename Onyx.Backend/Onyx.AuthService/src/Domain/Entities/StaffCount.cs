using Onyx.AuthService.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.AuthService.Domain.Entities
{
    public class UserCount : AuditableEntity
    {
        public int Count { get; set; }
        public int OrganizationId { get; set; }
    }
}
