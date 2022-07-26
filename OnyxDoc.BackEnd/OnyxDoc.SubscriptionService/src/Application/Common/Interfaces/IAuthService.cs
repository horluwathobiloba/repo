using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.Common.Interfaces
{
    public interface IAuthService
    {
        public SubscriberDto Subscriber { get; set; }
        public RoleDto Role { get; set; }
        public UserDto User { get; set; }
        public List<RoleDto> Roles { get; set; }
        public List<UserDto> Users { get; set; }
        public JobFunctionDto JobFunction { get; set; }
        public List<JobFunctionDto> JobFunctions { get; set; }
        public SystemSettingDto SystemSetting { get; set; }
        public string AuthToken { get; set; }

        Task<Tuple<EntityVm<SubscriberDto>, EntityVm<List<RoleDto>>, EntityVm<List<UserDto>>, EntityVm<List<JobFunctionDto>>>> GetSubscriberDataAsync(string authToken, int subscriberId, string userId, int skip = 0, int take = 0);
        Task<EntityVm<List<SubscriberDto>>> GetSubscribersAsync(string authToken, string userId, int skip = 0, int take = 0);
        Task<EntityVm<SubscriberDto>> GetSubscriberAsync(string authToken, int subscriberId, string userId);
        Task<EntityVm<List<RoleDto>>> GetRolesAsync(string authToken, int subscriberId, string userId, int skip = 0, int take = 0);
        Task<EntityVm<List<UserDto>>> GetUsersAsync(string authToken, int subscriberId, string userId, int skip = 0, int take = 0);
        Task<EntityVm<RoleDto>> GetRoleAsync(string authToken, int subscriberId, string userId, int roleId);
        Task<EntityVm<UserDto>> GetUserAsync(string authToken, int subscriberId, string userRecordId, string userId);
        Task<bool> IsValidUser(string authToken, int subscriberId, string userRecordId, string userId);
        Task<bool> IsValidSubscriber(string authToken, int subscriberId, string userId);
        Task<bool> IsValidRole(string authToken, int subscriberId, string userId, int roleId);
        Task<bool> ValidateRole(string authToken, int subscriberId, string userId, int roleId);
        Task<bool> IsValidJobFunction(string authToken, int subscriberId, string userId, int jobFunctionId);
        Task<bool> ValidateSubscriberData(string authToken, int SubscriberId, string userId, [Optional] int roleId, [Optional] int jobFunctionId);
        Task<bool> ValidateJobFunction(string authToken, int subscriberId, string userId, int jobFunctionId);
        Task<EntityVm<JobFunctionDto>> GetJobFunctionAsync(string authToken, int subscriberId, string userId, int jobFunctionId);
        Task<EntityVm<List<JobFunctionDto>>> GetJobFubctionsAsync(string authToken, int subscriberId, string userId, int skip = 0, int take = 0);
        Task<EntityVm<List<UserDto>>> GetAllUsersAsync(string authToken, int subscriberId, string userId, int skip = 0, int take = 0);
        Task<EntityVm<SystemSettingDto>> GetSystemSettingsAsync(string authToken, int subscriberId, string userId, int systemSettingsId);
    }
}
