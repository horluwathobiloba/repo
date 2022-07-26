using AutoMapper;
using OnyxDoc.SubscriptionService.Application.Common.Mappings;
using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.SubscriptionService.Application.PaymentChannels.Queries
{
    public class PaymentChannelDto : IMapFrom<PaymentChannel>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CurrencyId { get; set; }
        public int SubscriberId { get; set; }
        public string PaymentChannelName { get; set; }
        public CurrencyCode CurrencyCode { get; set; }
        public string CurrencyCodeDesc { get; set; }
        public decimal TransactionFee { get; set; }
        public TransactionRateType TransactionRateType { get; set; }
        public string UserId { get; set; }
             

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap< PaymentChannel, PaymentChannelDto>();
            profile.CreateMap<PaymentChannelDto, PaymentChannel>();
        }
    }
}
