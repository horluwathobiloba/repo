using OnyxDoc.AuthService.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.AuthService.Domain.Entities
{
    public class UserCount : AuditableEntity
    {
        public int Count { get; set; }
    }
}
