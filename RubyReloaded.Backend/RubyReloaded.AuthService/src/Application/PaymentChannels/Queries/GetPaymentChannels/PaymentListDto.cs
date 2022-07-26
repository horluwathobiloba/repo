using AutoMapper;
using RubyReloaded.AuthService.Application.Common.Mappings;
using RubyReloaded.AuthService.Domain.Entities;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.PaymentChannels.Queries.GetPaymentChannels
{
    public class PaymentChannelListDto : IMapFrom<PaymentChannel>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CurrencyCode { get; set; }
        public TransactionFeeType TransactionFeetype { get; set; }
        public decimal FeeRate { get; set; }
        public decimal MinTransactionFee { get; set; }
        public decimal MaxTransactionFee { get; set; }
        public bool AllowAutoReversal { get; set; }
        public decimal ReversalFeeRate { get; set; }
        public decimal ReversalTransactionFee { get; set; }
        public string PaymentChannelLogo { get; set; }
        public string PaymentChannelUrl { get; set; }
        public Status Status { get; set; }
        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<PaymentChannel, PaymentChannelListDto>();
            profile.CreateMap<PaymentChannelListDto, PaymentChannel>();

        }
    }
}
