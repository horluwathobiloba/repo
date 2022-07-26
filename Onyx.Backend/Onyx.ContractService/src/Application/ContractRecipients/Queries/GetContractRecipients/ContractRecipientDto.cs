using AutoMapper;
using Onyx.ContractService.Application.Common.Mappings;
using Onyx.ContractService.Application.Contracts.Queries.GetContracts;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.ContractRecipients.Queries.GetContractRecipients
{
    public class ContractRecipientDto : IMapFrom<ContractRecipient>
    {
        public int Id { get; set; }
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public int ContractId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public int Rank { get; set; }
        public string RecipientCategory { get; set; }


        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }
        public ContractDto Contract { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ContractRecipient, ContractRecipientDto>();
            profile.CreateMap<ContractRecipientDto, ContractRecipient>();
            profile.CreateMap<Domain.Entities.Contract, ContractDto>();
            profile.CreateMap<ContractDto, Domain.Entities.Contract>();
        }
    }
}
