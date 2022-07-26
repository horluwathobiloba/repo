using OnyxDoc.SubscriptionService.Domain.Common;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.SubscriptionService.Domain.Entities
{
    public class FlutterwavePaymentPlan : AuditableEntity
    {
        public int SubscriptionPlanId { get; set; }
        public int PaymentPlanId { get; set; }
        public string  CurrencyCode { get; set; }
        public decimal Amount { get; set; }
        public string Interval { get; set; }
        public string Duration { get; set; }
    }
   
}
