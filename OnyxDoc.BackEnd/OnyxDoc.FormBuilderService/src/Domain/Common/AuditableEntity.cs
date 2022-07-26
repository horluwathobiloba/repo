using Newtonsoft.Json;
using OnyxDoc.FormBuilderService.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnyxDoc.FormBuilderService.Domain.Common
{
    public abstract class AuditableEntity
    {

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByEmail { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }
        public string DeviceId { get; set; }
        //public int OrganisationId { get; set; }
        //public string OrganisationName { get; set; }
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }

        public string UserId { get; set; }
    }


}
