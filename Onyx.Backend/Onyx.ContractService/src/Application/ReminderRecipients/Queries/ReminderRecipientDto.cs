using AutoMapper;
using Onyx.ContractService.Application.Common.Mappings;
using Onyx.ContractService.Application.Contracts.Queries.GetContracts;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ReminderService.Application.ReminderRecipients.Queries.GetReminderRecipients
{
    public class ReminderRecipientDto : IMapFrom<ReminderRecipient>
    {
        public int Id { get; set; }
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public int ContractId { get; set; }
        public string Email { get; set; }
        public string Designation { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }
        public ContractDto Contract { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ReminderRecipient, ReminderRecipientDto>();
            profile.CreateMap<ReminderRecipientDto, ReminderRecipient>();
            profile.CreateMap<Contract, ContractDto>();
            profile.CreateMap<ContractDto, Contract>();
        }
    }
}
