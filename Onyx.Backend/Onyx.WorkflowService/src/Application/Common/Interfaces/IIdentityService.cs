using Onyx.WorkFlowService.Application.Common.Models;
using Onyx.WorkFlowService.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Onyx.WorkFlowService.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<(Result Result, string userName)> GetUserNameAsync(string userId);
        Task<(Result Result, Staff staff)> GetUserById(string userId);
        Task<(Result Result, Staff staff)> GetUserByIdAndOrganization(string userId, int organizationId);
        Task<(Result Result, Staff staff)> GetUserByUsername(string userName);
        Task<(Result Result, string UserId)> CreateStaffAsync(Staff staff);
        Task<Result> UpdateStaffAsync(Staff staff);
        Task<(Result result, List<Staff> staffs)> GetAll();
        Task<(Result result, List<Staff> staffs)> GetStaffsByOrganizationId(int organizationId);

        Task<(Result result, List<Staff> staffs)> GetStaffsByDepartmentId(int departmentId);
        Task<Result> ChangeStaffStatusAsync(Staff staff);
    }
}
