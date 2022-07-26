using Newtonsoft.Json;
using Paystack.Net;
using System;
using System.Collections.Generic;

namespace OnyxDoc.SubscriptionService.Application.Common.Models
{

    public class PaystackPriceResponse
    {
        [JsonProperty("name")]
        public bool Status { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("data")]
        public PriceData Data { get; set; }
    }

    public class PaystackPriceListResponse
    {
        [JsonProperty("name")]
        public bool Status { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("data")]
        public List<PriceData> Data { get; set; }
    }

    public class PriceData
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("price")]
        public int Price { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("is_shippable")]
        public bool IsShippable { get; set; }

        [JsonProperty("unlimited")]
        public bool Unlimited { get; set; }

        [JsonProperty("integration")]
        public int Integration { get; set; }

        [JsonProperty("domain")]
        public string Domain { get; set; }

        [JsonProperty("metadata")]
        public IDictionary<string, string> Metadata { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("product_code")]
        public string ProductCode { get; set; }

        [JsonProperty("quantity_sold")]
        public int QuantitySold { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("shipping_fields")]
        public Shipping_Fields ShippingFields { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("in_stock")]
        public bool InStock { get; set; }

        [JsonProperty("minimum_orderable")]
        public int MinimumOrderable { get; set; }

        [JsonProperty("maximum_orderable")]
        public object MaximumOrderable { get; set; }

        [JsonProperty("low_stock_alert")]
        public bool LowStockAlert { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
     
}
