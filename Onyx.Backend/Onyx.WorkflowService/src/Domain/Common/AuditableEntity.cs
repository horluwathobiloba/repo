using Onyx.WorkFlowService.Domain.Enums;
using System;

namespace Onyx.WorkFlowService.Domain.Common
{
    public abstract class AuditableEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
    }
}
