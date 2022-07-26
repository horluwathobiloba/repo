using AutoMapper;
using Onyx.ContractService.Application.Common.Mappings;
using Onyx.ContractService.Application.ContractRecipients.Queries.GetContractRecipients;
using Onyx.ContractService.Application.Contracts.Queries.GetContracts;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.ContractRecipientActions.Queries.GetContractRecipientActions
{
    public class ContractRecipientActionDto : IMapFrom<ContractRecipientAction>
    {
        public int Id { get; set; }
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; } 
        public int ContractId { get; set; }
        public int ContractRecipientId { get; set; }
        public string RecipientAction { get; set; }
        public string AppSigningUrl { get; set; }
        public string SignedDocumentUrl { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }
        public ContractRecipientDto ContractRecipient { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ContractRecipientAction, ContractRecipientActionDto>();
            profile.CreateMap<ContractRecipientActionDto, ContractRecipientAction>();
            profile.CreateMap<ContractRecipient, ContractRecipientDto>();
            profile.CreateMap<ContractRecipientDto, ContractRecipient>();
        }
    }
}
