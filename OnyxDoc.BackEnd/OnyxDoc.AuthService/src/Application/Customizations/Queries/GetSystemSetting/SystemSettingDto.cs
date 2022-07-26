using AutoMapper;
using OnyxDoc.AuthService.Application.Common.Mappings;
using OnyxDoc.AuthService.Application.Customizations.Queries.GetSystemSetting;
using OnyxDoc.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;

namespace OnyxDoc.AuthService.Application.SystemSetting.Queries.GetSystemSetting
{
    public class SystemSettingDto : IMapFrom<Domain.Entities.SystemSetting>
    {
        public ICollection<ExpiryPeriodDto> ExpirationReminder { get; set; } = new List<ExpiryPeriodDto>();
        public SettingsFrequency ExpirationSettingsFrequency { get; set; }
        public string ExpirationSettingsFrequencyDesc { get; set; }
        public int WorkflowReminder { get; set; }
        public SettingsFrequency WorkflowReminderSettingsFrequency { get; set; }
        public string WorkflowReminderSettingsFrequencyDesc { get; set; }
        public bool ShouldSentDocumentsExpire { get; set; }
        public int DocumentExpirationPeriod { get; set; }
        public string Currency { get; set; }
        public string Language { get; set; }
        public string CreatedById { get; set; }
        public string CreatedByEmail { get; set; }
        public int SubscriberId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.SystemSetting, SystemSettingDto>();
            profile.CreateMap<SystemSettingDto, Domain.Entities.SystemSetting>();

            //profile.CreateMap<Domain.Entities.ExpiryPeriod, ExpiryPeriodDto>();
            //profile.CreateMap<ExpiryPeriodDto, Domain.Entities.ExpiryPeriod>();
        }
    }
}
