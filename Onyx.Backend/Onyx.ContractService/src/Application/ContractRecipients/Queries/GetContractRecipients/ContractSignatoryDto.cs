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
    public class ContractSignatoryDto : IMapFrom<ContractRecipient>
    {
        public string Email { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ContractRecipient, ContractRecipientDto>();
            profile.CreateMap<ContractRecipientDto, ContractRecipient>();
            profile.CreateMap<Domain.Entities.Contract, ContractDto>();
            profile.CreateMap<ContractDto, Domain.Entities.Contract>();
        }
    }
}
