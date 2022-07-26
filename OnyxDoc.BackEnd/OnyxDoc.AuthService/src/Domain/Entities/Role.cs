using OnyxDoc.AuthService.Domain.Common;
using OnyxDoc.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.AuthService.Domain.Entities
{
    public class Role : AuditableEntity
    {
        public Subscriber Subscriber { get; set; }
        public RoleAccessLevel RoleAccessLevel { get; set; }
        public string RoleAccessLevelDesc { get; set; }
        public string WorkflowUserCategoryDesc { get; set; }
        public int CheckedCount { get; set; }

    }
}
