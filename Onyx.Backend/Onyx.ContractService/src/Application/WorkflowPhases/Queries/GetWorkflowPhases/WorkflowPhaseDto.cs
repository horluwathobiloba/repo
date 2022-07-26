using AutoMapper;
using Onyx.ContractService.Application.Common.Mappings;
using Onyx.ContractService.Application.ContractTypes.Queries.GetContractTypes;
using Onyx.ContractService.Application.WorkflowLevels.Queries.GetWorkflowLevels;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.WorkflowPhases.Queries.GetWorkflowPhases
{
    public class WorkflowPhaseDto : IMapFrom<WorkflowPhase>
    {
        public int Id { get; set; }
        public int OrganisationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string WorkflowSequence { get; set; }
        public string WorkflowUserCategory { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }

        public int ContractTypeId { get; set; }
        public ContractTypeDto ContractType { get; set; }
        public List<WorkflowLevelDto> WorkflowLevels { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<WorkflowPhase, WorkflowPhaseDto>();
            profile.CreateMap<WorkflowPhaseDto, WorkflowPhase>();
            profile.CreateMap<ContractType, ContractTypeDto>();
            profile.CreateMap<ContractTypeDto, ContractType>();
            profile.CreateMap<WorkflowLevel, WorkflowLevelDto>();
            profile.CreateMap<WorkflowLevelDto, WorkflowLevel>();
        }
    }
}
