using AutoMapper;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.ContractDuration.Queries.GetContractDurations
{
        public class ContractDurationDto
        {
        public int Id { get; set; }

        public string Name { get; set; }
        public DurationFrequency DurationFrequency { get; set; }
        public int Duration { get; set; }
        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.ContractDuration, ContractDurationDto>();
            profile.CreateMap<ContractDurationDto, Domain.Entities.ContractDuration>();
        }
    }
}
