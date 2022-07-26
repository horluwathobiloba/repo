using Newtonsoft.Json;
using Stripe;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.SubscriptionService.Application.Common.Models
{
    public class StripeProductResponse
    {
        [JsonProperty("product")]
        public Product Product { get; set; }
    }
}
