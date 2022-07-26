using Onyx.AuthService.Application.Common.Models;
using Onyx.AuthService.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Onyx.AuthService.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<(Result Result, string userName)> GetUserNameAsync(string userId);
        Task<(Result Result, User staff)> GetUserById(string userId);
        Task<(Result Result, User staff)> GetUserByEmail(string email);
        Task<(Result Result, User staff)> GetUserByIdAndOrganization(string userId, int organizationId);
        Task<(Result Result, User staff)> GetUserByIdOrganizationAndPermissions(string userId, int organizationId, int accessLevelId);
        Task<(Result Result, User staff)> GetUserByUsername(string userName);
        Task<(Result Result, string UserId)> CreateUserAsync(User staff);
        Task<Result> VerifyEmailAsync(User staff, string token);
        Task<Result> UpdateUserAsync(User staff);
        Task<(Result result, List<User> staffs)> GetAll();
        Task<(Result result, List<User> staffs)> GetUsersByOrganizationId(int organizationId);
        
        Task<(Result result, List<User> staffs)> GetUsersByDepartmentId(int departmentId);
        Task<(bool success, string token)> GenerateEmailToken(string email);
        Task<Result> ChangeUserStatusAsync(User staff);
        Task<(Result Result, List<User> users)> GetUsersByJobFunctionAndOrganization(int? JobfunctonId, int? organizationId);
    }
}
