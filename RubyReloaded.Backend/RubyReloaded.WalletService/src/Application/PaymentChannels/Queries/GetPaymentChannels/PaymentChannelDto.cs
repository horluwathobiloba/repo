
using AutoMapper;
using RubyReloaded.WalletService.Application.Common.Mappings;
using RubyReloaded.WalletService.Domain.Entities;
using RubyReloaded.WalletService.Domain.Enums;
using System;
using System.Collections.Generic;

namespace RubyReloaded.WalletService.Application.PaymentChannels.Queries.GetPaymentChannels
{
    public class PaymentChannelDto : IMapFrom<Domain.Entities.PaymentChannel>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public decimal PaymentGatewayFee { get; set; }
        public FeeType PaymentGatewayFeeType { get; set; }
        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }
        public string CurrencyCode { get; set; }
        public string SettlementAccountNumber { get; set; }
        public string Bank { get; set; }
        public PaymentChannelType PaymentChannelType { get; set; }
        public string PaymentChannelTypeDesc { get; set; }
        public PaymentGatewayCategory PaymentGatewayCategory { get; set; }
        public ICollection<BankService> BankServices { get; set; }
        public ICollection<PaymentGatewayService> PaymentGatewayServices { get; set; }
        public Status Status { get; set; }
        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<PaymentChannel, PaymentChannelDto>();
            profile.CreateMap<PaymentChannelDto, PaymentChannel>();
        }

    }
}