using OnyxDoc.FormBuilderService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.FormBuilderService.Domain.ViewModels.Common
{
        public abstract class AuthEntity
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string CreatedBy { get; set; }
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
        }

    public abstract class AuthObjectEntity
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public Status status { get; set; }
        public string statusDesc { get; set; }
        public string deviceId { get; set; }
        //public int OrganisationId { get; set; }
        //public string OrganisationName { get; set; }
        public int subscriberId { get; set; }
        public string subscriberName { get; set; }
    }

    public abstract class UserAuthEntity
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string CreatedBy { get; set; }
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
        }
 }

