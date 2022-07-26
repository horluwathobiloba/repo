using AutoMapper;
using System;
using Onyx.WorkFlowService.Domain.Entities;
using Onyx.WorkFlowService.Application.Common.Mappings;
using Onyx.WorkFlowService.Domain.Enums;
using Onyx.WorkFlowService.Domain.Common;

namespace Onyx.WorkFlowService.Application.Staffs.Queries.GetStaffs
{
    public class StaffDto : IMapFrom<Staff>
    {
        public string StaffId { get; set; }
        public int OrganizationId { get; set; }
        public int RoleId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public Gender Gender { get; set; }
        public string StaffCode { get; set; }
        public int MarketId { get; set; }
        public int DepartmentId { get; set; }
        public string SupervisorId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime EmploymentDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public DateTime LastAccessedDate { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string CurrentLocationAddress { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Staff, StaffDto>();
            profile.CreateMap<StaffDto, Staff>();
        }
    }
}
