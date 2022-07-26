using OnyxDoc.AuthService.Domain.Enums;
using System;



namespace OnyxDoc.AuthService.Domain.Common
{
    public abstract class AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SubscriberId { get; set; }
        public string CreatedByEmail { get; set; }
        public string CreatedById { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedById { get; set; }
        public string LastModifiedByEmail { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }
    }
}
