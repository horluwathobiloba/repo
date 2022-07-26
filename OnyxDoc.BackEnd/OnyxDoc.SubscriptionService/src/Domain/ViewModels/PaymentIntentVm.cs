using OnyxDoc.SubscriptionService.Domain.Common;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace OnyxDoc.SubscriptionService.Domain.ViewModels
{
   public class PaymentIntentVm : XPaymentIntent
    {
        public int SubscriberId { get; set; }

        public int PaymentPlanId { get; set; }
    }
}
