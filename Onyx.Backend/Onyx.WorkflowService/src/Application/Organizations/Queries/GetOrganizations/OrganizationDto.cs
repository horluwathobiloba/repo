using AutoMapper;
using System;
using Onyx.WorkFlowService.Domain.Entities;
using Onyx.WorkFlowService.Application.Common.Mappings;
using Onyx.WorkFlowService.Domain.Enums;
using Onyx.WorkFlowService.Domain.Common;

namespace Onyx.WorkFlowService.Application.Organizations.Queries.GetOrganizations
{
    public class OrganizationDto : IMapFrom<Organization>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RCNumber { get; set; }

        public string Address { get; set; }

        public string ContactEmail { get; set; }

        public string ContactPhoneNumber { get; set; }

        public string Code { get; set; }

        public string LogoFileLocation { get; set; }
        public string Country { get; set; }
        public string State { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<Organization, OrganizationDto>();
            profile.CreateMap<OrganizationDto, Organization>();
        }
    }
}
