using Onyx.AuthService.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.AuthService.Domain.Entities
{
    public class JobFunction:AuditableEntity
    {
        public int? OrganisationId { get; set; }
        public string UserId { get; set; }
    }
}
