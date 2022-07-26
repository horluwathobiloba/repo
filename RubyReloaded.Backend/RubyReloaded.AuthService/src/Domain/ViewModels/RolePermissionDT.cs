using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Domain.ViewModels
{
    public class RolePermissionsDT
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedById { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int Status { get; set; }
        public int RoleId { get; set; }
        public int CooperativeId { get; set; }
        public int AccessLevel { get; set; }
        public string Permission { get; set; }
    }
}
