using Onyx.ContractService.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Domain.Entities
{
    public class Dimension : AuditableEntity
    {
        public string Top { get; set; }
        public string Left { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
        public string Type { get; set; }
        public int Rank { get; set; }
        public int ContractId { get; set; }
        public string File { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public string Hash { get; set; }
        public int OrganizationId { get; set; }
        public string UserId { get; set; }
    }
}
