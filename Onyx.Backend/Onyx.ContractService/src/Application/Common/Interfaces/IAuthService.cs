using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.Common.Interfaces
{
    public interface IAuthService
    {
        public OrganisationDto Organisation { get; set; }
        public RoleDto Role { get; set; }
        public UserDto User { get; set; }
        public List<RoleDto> Roles { get; set; }
        public List<UserDto> Users { get; set; }
        public JobFunctionDto JobFunction { get; set; }
        public List<JobFunctionDto> JobFunctions { get; set; }
        public string AuthToken { get; set; }

        Task<Tuple<OrganisationVm, RoleListVm, UserListVm, JobFunctionListVm>> GetOrganisationDataAsync(string authToken,  int orgId);
        Task<OrganisationVm> GetOrganisationAsync(string authToken,  int orgId);
        Task<RoleListVm> GetRolesAsync(string authToken,  int orgId);
        Task<UserListVm> GetUsersAsync(string authToken,  int orgId);
        Task<RoleVm> GetRoleAsync(string authToken,  int roleId);
        Task<UserVm> GetUserAsync(string authToken, string userId);
        Task<bool> IsValidUser(string authToken, string userId);
        Task<bool> IsValidOrganisation(string authToken,  int orgId);
        Task<bool> IsValidRole(string authToken,  int roleId); 
        Task<bool> ValidateRole(string authToken,  int roleId);
        Task<bool> IsValidJobFunction(string authToken, int jobFunctionId);
        Task<bool> ValidateOrganisationData(string authToken, int organisationId, [Optional] int roleId, [Optional] int jobFunctionId);
        Task<bool> ValidateJobFunction(string authToken, int jobFunctionId);
        Task<JobFunctionVm> GetJobFunctionAsync(string authToken, int jobFunctionId);
        Task<JobFunctionListVm> GetJobFubctionsAsync(string authToken, int orgId);
        Task<UserListVm> GetAllUsersAsync();
    }
}
