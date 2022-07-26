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
    public class YearlyReminderScheduleDto : IMapFrom<YearlyReminderSchedule>
    {
        public int Id { get; set; }
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public int Value { get; set; }
        public string Month { get; set; }
        public int ReminderConfigurationId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<YearlyReminderSchedule, YearlyReminderScheduleDto>();
            profile.CreateMap<YearlyReminderScheduleDto, YearlyReminderSchedule>();
        }
    }
}
