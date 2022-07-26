using AutoMapper;
using System;
using Onyx.AuthService.Domain.Entities;
using Onyx.AuthService.Application.Common.Mappings;
using Onyx.AuthService.Domain.Enums;
using Onyx.AuthService.Domain.Common;

namespace Onyx.AuthService.Application.JobFunctions.Queries.GetJobFunctions
{
    public class JobFunctionDto : IMapFrom<JobFunction>
    {
        public int? OrganisationId { get; set; }
        public string UserId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<JobFunction, JobFunctionDto>();
            profile.CreateMap<JobFunctionDto, JobFunction>();
        }
    }
}
