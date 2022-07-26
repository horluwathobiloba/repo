using OnyxDoc.AuthService.Domain.Common;
using OnyxDoc.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.AuthService.Domain.Entities
{
    public class SubscriberCount : AuditableEntity
    {
        public int Count { get; set; }
    }
}
