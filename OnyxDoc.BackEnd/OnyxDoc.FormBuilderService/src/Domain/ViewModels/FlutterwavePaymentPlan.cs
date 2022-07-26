using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.FormBuilderService.Domain.ViewModels
{
  
    public class FlutterwavePaymentPlanResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        public PaymentPlanData data { get; set; }
    }

    public class PaymentPlanData
    {
        public int id { get; set; }
        public string name { get; set; }
        public decimal  amount { get; set; }
        public string interval { get; set; }
        public int duration { get; set; }
        public string status { get; set; }
        public string currency { get; set; }
        public string plan_token { get; set; }
        public DateTime created_at { get; set; }
    }
}
