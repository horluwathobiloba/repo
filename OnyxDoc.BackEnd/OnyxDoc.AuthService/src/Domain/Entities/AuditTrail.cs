
using OnyxDoc.AuthService.Domain.Common;
using OnyxDoc.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.AuthService.Domain.Entities
{
    public class AuditTrail : AuditableEntity
    {
        public string UserId { get; set; }
        public AuditAction AuditAction { get; set; }
        public string AuditActionDesc { get; set; }
        public string ControllerName { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
        public string AffectedColumns { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string MicroserviceName { get; set; }

    }
}
