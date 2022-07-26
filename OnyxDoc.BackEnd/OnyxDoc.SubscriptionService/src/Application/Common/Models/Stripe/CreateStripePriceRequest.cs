using OnyxDoc.SubscriptionService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.SubscriptionService.Application.Common.Models
{
    public class CreateStripePriceRequest
    {
        public SubscriptionPlanPricing Price { get; set; }
    }
}
