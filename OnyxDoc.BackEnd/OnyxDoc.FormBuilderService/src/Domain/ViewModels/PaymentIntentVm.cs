using OnyxDoc.FormBuilderService.Domain.Common;
using OnyxDoc.FormBuilderService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace OnyxDoc.FormBuilderService.Domain.ViewModels
{
   public class PaymentIntentVm : XPaymentIntent
    {
        public int SubscriberId { get; set; }

        public int PaymentPlanId { get; set; }
    }
}
