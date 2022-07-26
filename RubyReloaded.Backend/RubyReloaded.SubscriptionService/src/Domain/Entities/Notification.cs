using RubyReloaded.SubscriptionService.Domain.Common;
using RubyReloaded.SubscriptionService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.SubscriptionService.Domain.Entities
{
    public class Notification:AuditableEntity
    {
        public string CustomerId { get; set; }
        public string Message { get; set; }
        //public string DeviceId { get; set; }
        public NotificationStatus NotificationStatus { get; set; }
        public ApplicationType ApplicationType { get; set; }

    }
}
