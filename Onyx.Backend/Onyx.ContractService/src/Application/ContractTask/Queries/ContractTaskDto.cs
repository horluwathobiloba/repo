using AutoMapper;
using Onyx.ContractService.Application.Common.Mappings;
using Onyx.ContractService.Application.ContractTypeInitiators.Queries.GetContractTypeInitiators;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Onyx.ContractService.Application.ContractTask.Queries
{
    public class ContractTaskDto : IMapFrom<Domain.Entities.ContractTask>
    {
        public int ContractId { get; set; }
        public DateTime DueDate { get; set; }
        public string TaskCreatedById { get; set; }
        public string AssignedUserId { get; set; }
        public int Id { get; set; }
        public string AssignedUserEmail { get; set; }
        //public string TaskCompletedByName { get; set; }
        //public string TaskCompletedByEmail { get; set; }
        public ContractTaskStatus ContractTaskStatus { get; set; }

        
        //public Domain.Entities.Contract Contract { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.ContractTask, ContractTaskDto>();
            profile.CreateMap<ContractTaskDto, Domain.Entities.ContractTask>();
        }
    }
}
