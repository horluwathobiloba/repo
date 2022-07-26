using AutoMapper;
using OnyxDoc.AuthService.Application.Common.Mappings;
using OnyxDoc.AuthService.Application.SystemSetting.Queries.GetSystemSetting;
using OnyxDoc.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.AuthService.Application.Customizations.Queries.GetSystemSetting
{
    public class ExpiryPeriodDto : IMapFrom<Domain.Entities.ExpiryPeriod>
    {
        public int ExpirationReminderInterval { get; set; }
        public SettingsFrequency ExpirationSettingsFrequency { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.ExpiryPeriod, ExpiryPeriodDto>();
            profile.CreateMap<ExpiryPeriodDto, Domain.Entities.ExpiryPeriod>();
        }
    }
}
