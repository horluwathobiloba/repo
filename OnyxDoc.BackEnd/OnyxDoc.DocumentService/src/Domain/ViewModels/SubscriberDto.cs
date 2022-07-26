
using OnyxDoc.DocumentService.Domain.Common;
using OnyxDoc.DocumentService.Domain.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnyxDoc.DocumentService.Domain.ViewModels
{

  
    public class SubscriberDto : AuthEntity
    {
        public string PhoneNumber { get; set; }
        public string ContactEmail { get; set; }
        public string Email { get; set; }
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
    }

    public class SubscriberObjectDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public string phoneNumber { get; set; }
        public string contactEmail { get; set; }
        public string email { get; set; }
        public string subscriberCode { get; set; }
        public string staffSize { get; set; }
        public string industry { get; set; }
        public string referrer { get; set; }
        public string themeColor { get; set; }
        public string subscriptionPurpose { get; set; }
        public SubscriberType subscriberType { get; set; }
        public string subscriberTypeDesc { get; set; }
        public SubscriberAccessLevel subscriberAccessLevel { get; set; }
        public string subscriberAccessLevelDesc { get; set; }
        public bool hasActivatedFreeTrial { get; set; }
        public bool freeTrialCompleted { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string logo { get; set; }
    }
}


