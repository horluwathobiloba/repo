
using AutoMapper;
using OnyxDoc.AuthService.Application.Common.Mappings;
using OnyxDoc.AuthService.Domain.Common;
using OnyxDoc.AuthService.Domain.Entities;
using OnyxDoc.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;

namespace OnyxDoc.AuthService.Application.Subscribers.Queries.GetSubscribers
{
    public class SubscriberListDto : IMapFrom<Subscriber>
{
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string ContactEmail { get; set; }
        public string SubscriberCode { get; set; }
        public string StaffSize { get; set; }
        public string Industry { get; set; }
        public string Referrer { get; set; }
        public string ThemeColor { get; set; }
        public string SubscriptionPurpose { get; set; }
        public SubscriberType SubscriberType { get; set; }
        public string SubscriberTypeDesc { get; set; }
        public SubscriberAccessLevel SubscriberAccessLevel { get; set; }
        public string SubscriberAccessLevelDesc { get; set; }
        public bool HasActivatedFreeTrial { get; set; }
        public bool FreeTrialCompleted { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Logo { get; set; }
        public string CreatedById { get; set; }
        public string CreatedByEmail { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }

        public string SubscriptionStatus { get; set; }
        public decimal Amount { get; set; }
        public string Period { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Subscriber, SubscriberListDto> ();
            profile.CreateMap<SubscriberListDto, Subscriber>();
        }
    }
}
