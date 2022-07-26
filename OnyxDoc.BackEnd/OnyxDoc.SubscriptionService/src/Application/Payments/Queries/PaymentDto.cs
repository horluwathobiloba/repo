using OnyxDoc.SubscriptionService.Application.Common.Mappings;
using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Domain.Enums;
using AutoMapper;
using System;
using System.Collections.Generic;

namespace OnyxDoc.SubscriptionService.Application.Payments.Queries.GetPayments
{
    public class PaymentDto : IMapFrom<Payment>
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public string OrderNo { get; set; }
        public string SessionId { get; set; }
        public string PaymentMethodType { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public decimal TransactionFee { get; set; }
        public long Quantity { get; set; }
        public string Mode { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Payment, PaymentDto>();
            profile.CreateMap<PaymentDto, Payment>();
        }
    }
}
