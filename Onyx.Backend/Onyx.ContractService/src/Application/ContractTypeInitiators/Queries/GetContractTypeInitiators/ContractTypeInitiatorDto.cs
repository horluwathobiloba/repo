using AutoMapper;
using Onyx.ContractService.Application.Common.Mappings;
using Onyx.ContractService.Application.ContractTypes.Queries.GetContractTypes;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.ContractTypeInitiators.Queries.GetContractTypeInitiators
{
    public class ContractTypeInitiatorDto : IMapFrom<ContractTypeInitiator>
    {
        public int Id { get; set; }
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public int ContractTypeId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }
        public ContractTypeDto ContractType { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ContractTypeInitiator, ContractTypeInitiatorDto>();
            profile.CreateMap<ContractTypeInitiatorDto, ContractTypeInitiator>();
            profile.CreateMap<ContractType, ContractTypeDto>();
            profile.CreateMap<ContractTypeDto, ContractType>();
        }
    }
}
