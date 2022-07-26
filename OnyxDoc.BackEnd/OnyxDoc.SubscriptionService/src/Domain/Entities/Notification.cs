using OnyxDoc.SubscriptionService.Domain.Common;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.SubscriptionService.Domain.Entities
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
