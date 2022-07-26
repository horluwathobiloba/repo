using Onyx.AuthService.Domain.Common;
using Onyx.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.AuthService.Domain.Entities
{
    public class Role : AuditableEntity
    {
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public string AccessLevelDesc { get; set; }
        public List<RolePermission> RolePermissions { get; set; }
        public WorkflowUserCategory WorkflowUserCategory { get; set; }
        public string WorkflowUserCategoryDesc { get; set; }

    }
}
