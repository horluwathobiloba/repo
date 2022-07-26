using OnyxDoc.AuthService.Domain.Common;
using OnyxDoc.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;



namespace OnyxDoc.AuthService.Domain.Entities
{
    public class Subscriber : AuditableEntity
    {
        public string PhoneNumber { get; set; }
        public string ContactEmail { get; set; }
        public string SubscriberCode { get; set; }
        public string StaffSize { get; set; }
        public string Industry { get; set; }
        public string Referrer { get; set; }
        public ReferralSource? ReferralSource { get; set; }
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
        public int SubscriptionPlan { get; set; }
        public int Amount { get; set; }
        public string Period { get; set; }
    }
}