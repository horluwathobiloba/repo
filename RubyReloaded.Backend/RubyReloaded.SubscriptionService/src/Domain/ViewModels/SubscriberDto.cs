using RubyReloaded.SubscriptionService.Domain.Common;
using RubyReloaded.SubscriptionService.Domain.Enums;
using RubyReloaded.SubscriptionService.Domain.ViewModels;

namespace RubyReloaded.SubscriptionService.Domain.ViewModels
{
    public class SubscriberDto : AuthEntity
    {
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string ContactEmail { get; set; }
        public string SubscriberCode { get; set; }
        public string StaffSize { get; set; }
        public string Industry { get; set; }
        public string Location { get; set; }
        public string Referrer { get; set; }
        public string SubscriptionPurpose { get; set; }
        public SubscriberType SubscriberType { get; set; }
        public string SubscriberTypeDesc { get; set; }
        public SubscriberAccessLevel SubscriberAccessLevel { get; set; }
        public string SubscriberAccessLevelDesc { get; set; }
        public string Address { get; set; }
    }
}
