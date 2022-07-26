using OnyxDoc.SubscriptionService.Domain.Common;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.SubscriptionService.Domain.Entities
{
    public class Inbox:AuditableEntity
    {
        public string Body { get; set; }
        public string ReciepientEmail { get; set; }
        public bool Delivered { get; set; }
        public bool Read { get; set; }
        public EmailAction EmailAction { get; set; }
        public string Sender { get; set; }

    }
}
