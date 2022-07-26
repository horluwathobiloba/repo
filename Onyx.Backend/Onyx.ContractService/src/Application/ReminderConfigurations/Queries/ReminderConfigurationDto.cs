using AutoMapper;
using Onyx.ContractService.Application.Common.Mappings;
using Onyx.ContractService.Application.Contracts.Queries.GetContracts;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ConractService.Application.ReminderConfigurations.Queries.GetReminderConfigurations
{
    public class ReminderConfigurationDto : IMapFrom<ReminderConfiguration>
    {
        public int Id { get; set; }
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int RecurrenceValue { get; set; }
        public ReminderScheduleFrequency ReminderScheduleFrequency { get; set; }
        public List<WeeklyReminderSchedule> WeeklyReminderSchedule { get; set; }
        public string MonthlyReminderScheduleValue { get; set; }
        public YearlyReminderSchedule YearlyReminderScheduleValue { get; set; }
        public ReminderType ReminderType { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ReminderConfiguration, ReminderConfigurationDto>();
            profile.CreateMap<ReminderConfigurationDto, ReminderConfiguration>();
        }
    }
}
