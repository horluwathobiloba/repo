using AutoMapper;
using Onyx.ContractService.Application.Common.Mappings;
using Onyx.ContractService.Application.ContractTypes.Queries.GetContractTypes;
using Onyx.ContractService.Application.WorkflowPhases.Queries.GetWorkflowPhases;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.WorkflowLevels.Queries.GetWorkflowLevels
{
    public class WorkflowLevelDto : IMapFrom<WorkflowLevel>
    {
        public WorkflowLevelDto()
        { }
        public WorkflowLevelDto(WorkflowLevelDto x,  RoleDto role)
        {
            this.Id = x.Id;
            this.OrganisationId = x.OrganisationId;
            this.RoleId = x.RoleId;
            this.Rank = x.Rank;
            this.WorkflowLevelAction = x.WorkflowLevelAction;
            this.CreatedBy = x.CreatedBy;
            this.CreatedDate = x.CreatedDate;
            this.LastModifiedBy = x.LastModifiedBy;
            this.LastModifiedDate = x.LastModifiedDate;
            this.Status = x.Status;
            this.StatusDesc = x.StatusDesc;
            this.WorkflowPhaseId = x.WorkflowPhaseId;
            this.Role = role;
            this.WorkflowPhase = x.WorkflowPhase;
        }

        public int Id { get; set; }
        public int OrganisationId { get; set; }
        public int RoleId { get; set; }
        public int Rank { get; set; }
        public string WorkflowLevelAction { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }
        public int WorkflowPhaseId { get; set; }
        public RoleDto Role { get; set; }
        public WorkflowPhaseDto WorkflowPhase { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<WorkflowLevel, WorkflowLevelDto>();
            profile.CreateMap<WorkflowLevelDto, WorkflowLevel>();
            profile.CreateMap<WorkflowPhase, WorkflowPhaseDto>();
            profile.CreateMap<WorkflowPhaseDto, WorkflowPhase>();
        }
    }
}
