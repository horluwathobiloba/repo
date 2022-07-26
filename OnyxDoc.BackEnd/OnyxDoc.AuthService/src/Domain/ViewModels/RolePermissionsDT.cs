using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.AuthService.Domain.ViewModels
{
    public class RolePermissionsDT
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string CreatedBy { get; set; }
        public string CreatedById { get; set; }

        public string CreatedByEmail { get; set; }
        public string Category { get; set; }
        public DateTime CreatedDate { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
        public int Status { get; set; }
        public string StatusDesc { get; set; }
        public int RoleId { get; set; }
        public int SubscriberId { get; set; }
        public int RoleAccessLevel { get; set; }

        public string Permission { get; set; }

    }
}
