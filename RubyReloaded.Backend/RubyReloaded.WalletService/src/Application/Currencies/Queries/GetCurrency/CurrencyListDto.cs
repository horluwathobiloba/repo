
using AutoMapper;
using RubyReloaded.WalletService.Application.Common.Mappings;
using RubyReloaded.WalletService.Domain.Entities;
using RubyReloaded.WalletService.Domain.Enums;
using System;

namespace RubyReloaded.WalletService.Application.Currencys.Queries.GetCurrency
{
    public class CurrencyListDto : IMapFrom<Currency>
    {
        public int Id { get; set; }
        public CurrencyCode CurrencyCode { get; set; }
        public string CurrencyCodeString { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Currency, CurrencyListDto>();
            profile.CreateMap<CurrencyListDto, Currency>();
        }
    }
}
