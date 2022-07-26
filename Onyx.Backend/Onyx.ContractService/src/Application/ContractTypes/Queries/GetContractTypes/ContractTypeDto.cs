using AutoMapper;
using Onyx.ContractService.Application.Common.Mappings;
using Onyx.ContractService.Application.ContractTypeInitiators.Queries.GetContractTypeInitiators;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.ContractTypes.Queries.GetContractTypes
{
    public class ContractTypeDto : IMapFrom<ContractType>
    {
        public int Id { get; set; }
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        //public bool EnableInternalSignatories { get; set; }
        //public int NumberOfInternalSignatories { get; set; }
        //public bool EnableExternalSignatories { get; set; }
        //public int NumberOfExternalSignatories { get; set; }
        //public bool EnableThirdPartySignatories { get; set; }
        //public int NumberOfThirdPartySignatories { get; set; }
        //public bool EnableWitnessSignatories { get; set; }
        //public int NumberOfWitnessSignatories { get; set; }
        public string TemplateFilePath { get; set; }
        public ICollection<ContractTypeInitiator> ContractTypeInitiators { get; set; } 
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ContractType, ContractTypeDto>();
            profile.CreateMap<ContractTypeDto, ContractType>();
        }
    }
}
