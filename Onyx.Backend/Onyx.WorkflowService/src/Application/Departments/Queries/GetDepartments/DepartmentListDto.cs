
using AutoMapper;
using Onyx.WorkFlowService.Application.Common.Mappings;
using Onyx.WorkFlowService.Domain.Common;
using Onyx.WorkFlowService.Domain.Entities;
using Onyx.WorkFlowService.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Onyx.WorkFlowService.Application.Departments.Queries.GetDepartments
{
    public class DepartmentListDto :  IMapFrom<Department>
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
            profile.CreateMap<Department, DepartmentListDto> ();
            profile.CreateMap<DepartmentListDto, Department>();
        }
    }
}
