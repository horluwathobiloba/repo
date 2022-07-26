﻿using AutoMapper;
using RubyReloaded.SubscriptionService.Application.Common.Mappings;
using RubyReloaded.SubscriptionService.Domain.Entities;
using RubyReloaded.SubscriptionService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.SubscriptionService.Application.Currencies.Queries
{
    public class CurrencyDto : IMapFrom<Domain.Entities.Currency>
    {
        public int Id { get; set; }
        public CurrencyCode CurrencyCode { get; set; }
        public string CurrencyCodeDesc { get; set; } 
        public bool IsDefault { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Currency, CurrencyDto>();
            profile.CreateMap<CurrencyDto, Domain.Entities.Currency>();
        }
    }
}
