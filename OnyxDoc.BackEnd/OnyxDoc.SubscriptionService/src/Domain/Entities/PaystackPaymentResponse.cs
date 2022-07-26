using Newtonsoft.Json;
using OnyxDoc.SubscriptionService.Domain.Common;
using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OnyxDoc.SubscriptionService.Application.Common.Models
{
    public class PaystackPaymentResponse : AuditableEntity
    {
        public int PaymentChannelId { get; set; }
        [ForeignKey(nameof(PaymentChannelId))]
        public PaymentChannel PaymentChannel { get; set; }
        public bool StatusFlag { get; set; }
        public string Message { get; set; }

        #region Data
        public string Authorization_Url { get; set; }
        public string Access_Code { get; set; }
        public string Reference { get; set; }
        public string Domain { get; set; }
        public string Data_Status { get; set; }
        public string Gateway_Response { get; set; }
        public string Card { get; set; }
        public string Currency { get; set; }
        public decimal? Fees { get; set; }
        public decimal? FeeSplit { get; set; }
        public decimal? Requested_Amount { get; set; }
        #endregion

        #region Authorization
        public string Authorization_Code { get; set; }
        public string Bin { get; set; }
        public string Last4 { get; set; }
        public string Exp_Month { get; set; }
        public string Exp_Year { get; set; }
        public string Channel { get; set; }
        public string Card_Type { get; set; }
        public string Bank { get; set; }
        public string Country_Code { get; set; }
        public string Brand { get; set; }
        public string Signature { get; set; }
        public string Account_Name { get; set; }
        #endregion 

        public PaymentStatus PaymentStatus { get; set; }
        public bool LiveMode { get; set; }

    }
}


