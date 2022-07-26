using AutoMapper;
using RubyReloaded.AuthService.Application.Common.Mappings;
using RubyReloaded.AuthService.Domain.Entities;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.CurrencyConfigurations.Queries.GetCurrencyConfigurations
{
    public class CurrencyConfigurationListDto : IMapFrom<CurrencyConfiguration>
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
            profile.CreateMap<CurrencyConfiguration, CurrencyConfigurationListDto>();
            profile.CreateMap<CurrencyConfigurationListDto, CurrencyConfiguration>();
        }
    }
}
