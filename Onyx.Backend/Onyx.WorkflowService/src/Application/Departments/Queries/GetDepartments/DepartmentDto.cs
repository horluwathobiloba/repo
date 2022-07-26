using AutoMapper;
using System;
using Onyx.WorkFlowService.Domain.Entities;
using Onyx.WorkFlowService.Application.Common.Mappings;
using Onyx.WorkFlowService.Domain.Enums;
using Onyx.WorkFlowService.Domain.Common;

namespace Onyx.WorkFlowService.Application.Departments.Queries.GetDepartments
{
    public class DepartmentDto : IMapFrom<Department>
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public string Name { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Department, DepartmentDto>();
            profile.CreateMap<DepartmentDto, Department>();
        }
    }
}
