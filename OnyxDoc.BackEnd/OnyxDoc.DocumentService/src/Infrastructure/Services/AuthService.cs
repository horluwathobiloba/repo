using Microsoft.Extensions.Configuration;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Documents.Queries.GetDocuments;
using OnyxDoc.DocumentService.Domain.ViewModels;
using OnyxDoc.DocumentService.Infrastructure.Utility;
using RestSharp;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace OnyxDoc.DocumentService.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IBearerTokenService _bearerTokenService;
        private readonly IConfiguration _configuration;
        private readonly IRestClient _client;
        private readonly IAPIClientService _apiClient;

        public SystemSettingDto SystenSetting { get; set; }
        public SubscriberDto Subscriber { get; set; }
        public JobFunctionDto JobFunction { get; set; }
        public RoleDto Role { get; set; }
        public UserDto User { get; set; }
        public List<RoleDto> Roles { get; set; }
        public List<UserDto> Users { get; set; }
        public List<JobFunctionDto> JobFunctions { get; set; }

        public int SubscriberId { get; set; }
        public string UserId { get; set; }
        public string JobFunctionId { get; set; }
        public string AuthToken { get; set; }


        public AuthService(IConfiguration configuration, IRestClient client, IAPIClientService apiClient)
        {
            _configuration = configuration;
            _client = client;
            _apiClient = apiClient;
        }

        public async Task<Tuple<EntityVm<SubscriberDto>, EntityVm<List<RoleDto>>, EntityVm<List<UserDto>>, EntityVm<List<JobFunctionDto>>>>
            GetSubscriberDataAsync(string authToken, int subscriberId, string userId, int skip = 0, int take = 0)
        {
            try
            {
                this.SetAuthToken(authToken);
                this.UserId = userId;

                var subscriberApiUrl = _configuration["AuthService:GetSubscriberApiUrl"] + subscriberId + "/" + userId;
                var rolesApiUrl = _configuration["AuthService:GetRolesApiUrl"] + subscriberId + "/" + userId + "/" + skip + "/" + take;
                var usersApiUrl = _configuration["AuthService:GetUsersApiUrl"] + subscriberId + "/" + userId + "/" + skip + "/" + take;
                var jobFunctionsApiUrl = _configuration["AuthService:GetJobFunctionsApiUrl"] + subscriberId + "/" + userId + "/" + "/" + skip + "/" + take;

                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                var org = await _apiClient.Get<EntityVm<SubscriberDto>>(subscriberApiUrl, this.AuthToken);
                var roles = await _apiClient.Get<EntityVm<List<RoleDto>>>(rolesApiUrl, this.AuthToken);
                var users = await _apiClient.Get<EntityVm<List<UserDto>>>(usersApiUrl, this.AuthToken);
                var jobFunctions = await _apiClient.Get<EntityVm<List<JobFunctionDto>>>(jobFunctionsApiUrl, this.AuthToken);

                var result = new Tuple<EntityVm<SubscriberDto>, EntityVm<List<RoleDto>>, EntityVm<List<UserDto>>, EntityVm<List<JobFunctionDto>>>(org, roles, users, jobFunctions);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EntityVm<List<SubscriberDto>>> GetSubscribersAsync(string authToken, string userId, int skip = 0, int take = 0)
        {
            try
            {
                this.SetAuthToken(authToken);
                var subscriberApiUrl = _configuration["AuthService:GetSubscribersApiUrl"] + userId + "/" + skip + "/" + take;
                var subscriber = await _apiClient.Get<EntityVm<List<SubscriberDto>>>(subscriberApiUrl, this.AuthToken?.Split(" ")[1]);
                return subscriber;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EntityVm<SubscriberDto>> GetSubscriberAsync(string authToken, int subscriberId, string userId)
        {
            try
            {
                this.SetAuthToken(authToken);
                var subscriberApiUrl = _configuration["AuthService:GetSubscriberApiUrl"] + subscriberId + "/" + userId;
                var subscriber = await _apiClient.Get<EntityVm<SubscriberDto>>(subscriberApiUrl, this.AuthToken?.Split(" ")[1]);
                this.Subscriber = subscriber.entity;
                return subscriber;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EntityVm<List<RoleDto>>> GetRolesAsync(string authToken, int subscriberId, string userId, int skip = 0, int take = 0)
        {
            try
            {
                this.SetAuthToken(authToken);
                var rolesApiUrl = _configuration["AuthService:GetRolesApiUrl"] + subscriberId + "/" + userId + "/" + skip + "/" + take;
                var roles = await _apiClient.Get<EntityVm<List<RoleDto>>>(rolesApiUrl, this.AuthToken?.Split(" ")[1]);
                this.Roles = roles.entity;
                return roles;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EntityVm<List<JobFunctionDto>>> GetJobFubctionsAsync(string authToken, int subscriberId, string userId, int skip = 0, int take = 0)
        {
            try
            {
                this.SetAuthToken(authToken);
                var rolesApiUrl = _configuration["AuthService:GetJobFunctionsApiUrl"] + subscriberId + "/" + userId + "/" + skip + "/" + take;
                var jobFunctions = await _apiClient.Get<EntityVm<List<JobFunctionDto>>>(rolesApiUrl, this.AuthToken?.Split(" ")[1]);
                this.JobFunctions = jobFunctions.entity;
                return jobFunctions;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public async Task<EntityVm<JobFunctionDto>> GetJobFunctionAsync(string authToken, int subscriberId, string userId, int jobFunctionId)
        {
            try
            {
                this.SetAuthToken(authToken);
                var roleApiUrl = _configuration["AuthService:GetJobFunctionApiUrl"] + subscriberId + "/" + userId + "/" + jobFunctionId;
                var jobFunction = await _apiClient.Get<EntityVm<JobFunctionDto>>(roleApiUrl, this.AuthToken?.Split(" ")[1]);
                return jobFunction;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EntityVm<List<UserDto>>> GetUsersAsync(string authToken, int subscriberId, string userId, int skip = 0, int take = 0)
        {
            try
            {
                this.SetAuthToken(authToken);
                var usersApiUrl = _configuration["AuthService:GetUsersApiUrl"] + subscriberId + "/" + userId + "/" + skip + "/" + take;
                var users = await _apiClient.Get<EntityVm<List<UserDto>>>(usersApiUrl, this.AuthToken?.Split(" ")[1]);
                this.Users = users.entity;
                return users;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public async Task<EntityVm<List<UserDto>>> GetAllUsersAsync(string authToken, int subscriberId, string userId, int skip = 0, int take = 0)
        {
            try
            {
                this.SetAuthToken(authToken);
                var usersApiUrl = _configuration["AuthService:GetAllUsers"] + subscriberId + "/" + userId + "/" + skip + "/" + take;
                var users = await _apiClient.Get<EntityVm<List<UserDto>>>(usersApiUrl, "");
                return users;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EntityVm<RoleDto>> GetRoleAsync(string authToken, int subscriberId, string userId, int roleId)
        {
            try
            {
                this.SetAuthToken(authToken);
                var roleApiUrl = _configuration["AuthService:GetRoleApiUrl"] + roleId + "/" + subscriberId + "/" + userId;
                var role = await _apiClient.Get<EntityVm<RoleDto>>(roleApiUrl, this.AuthToken?.Split(" ")[1]);
                return role;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EntityVm<UserDto>> GetUserAsync(string authToken, int subscriberId, string userRecordId, string userId)
        {
            try
            {
                this.SetAuthToken(authToken);
                var userApiUrl = _configuration["AuthService:GetUserApiUrl"] + userRecordId + "/" + subscriberId + "/" + userId;
                var user = await _apiClient.Get<EntityVm<UserDto>>(userApiUrl, this.AuthToken?.Split(" ")[1]);
                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> IsValidUser(string authToken, int subscriberId, string userRecordId, string userId)
        {
            try
            {
                this.SetAuthToken(authToken);
                this.UserId = userId;
                var user = await GetUserAsync(authToken, subscriberId, userRecordId, userId);
                if (user == null || string.IsNullOrEmpty(user.entity.Id))
                {
                    throw new Exception("Invalid role specified.");
                }
                return !string.IsNullOrEmpty(user.entity.Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> IsValidRole(string authToken, int subscriberId, string userId, int roleId)
        {
            try
            {
                this.SetAuthToken(authToken);
                this.UserId = userId;
                var role = await GetRoleAsync(authToken, subscriberId, userId, roleId);
                if (role == null || role.entity.Id <= 0)
                {
                    throw new Exception("Invalid role specified.");
                }
                this.Role = role.entity;
                return role != null && role.entity.Id > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> IsValidJobFunction(string authToken, int subscriberId, string userId, int jobFunctionId)
        {
            try
            {
                this.SetAuthToken(authToken);
                this.UserId = userId;
                var jobFunction = await GetJobFunctionAsync(authToken, subscriberId, userId, jobFunctionId);
                if (jobFunction == null || jobFunction.entity.Id <= 0)
                {
                    throw new Exception("Invalid job function specified.");
                }
                this.JobFunction = jobFunction.entity;
                return jobFunction != null && jobFunction.entity.Id > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> IsValidSubscriber(string authToken, int subscriberId, string userId)
        {
            try
            {
                var org = await GetSubscriberAsync(authToken, subscriberId, userId);
                if (org == null || org.entity.Id <= 0)
                {
                    throw new Exception("Invalid subscriber specified.");
                }
                return org != null && org.entity.Id > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetAuthToken(string authToken)
        {
            if (string.IsNullOrEmpty(authToken))
            {
                throw new Exception("Access token not found! Value is null.");
            }
            if (string.IsNullOrEmpty(this.AuthToken))
            {
                this.AuthToken = authToken;
                var accessToken = this.AuthToken?.ExtractToken();
                var uid = accessToken.Claims.FirstOrDefault(claim => claim.Type == "userid");
                var userId = uid == null ? "" : uid?.Value;
                var subscriberId = accessToken.Claims.FirstOrDefault(claim => claim.Type == "subscriberId")?.Value;
                if (subscriberId != null)
                {
                    this.SubscriberId = subscriberId.ToInt();
                }
                if (userId != null)
                {
                    this.UserId = userId;
                }
            }
        }

        protected async Task<string> GetBearerToken()
        {
            string appName = _configuration["AuthService:appName"];
            string tokenApiUrl = _configuration["AuthService:tokenApiUrl"];
            var token = await _bearerTokenService.GetBearerToken(tokenApiUrl, new { name = appName });
            return token;
        }

        public async Task<bool> ValidateSubscriberData(string authToken, int subscriberId, string userId, [Optional] int roleId, [Optional] int jobFunctionId)
        {
            if (subscriberId <= 0)
            {
                throw new Exception($"Subscriber id must be specified!");
            }

            var orgResponse = subscriberId <= 0 ? null : await this.GetSubscriberAsync(authToken, subscriberId, userId);

            //mandatory
            if (orgResponse == null || !orgResponse.succeeded || orgResponse.entity == null)
            {
                throw new Exception($"Subscriber specified is invalid!");
            }

            //optionally mandatory
            if (userId != "null" && !userId.Contains("@"))
            {
                if (!string.IsNullOrEmpty(this.UserId))
                {
                    var userResponse = await this.GetUserAsync(authToken, subscriberId, userId, userId);
                    if (userResponse == null || !userResponse.succeeded)
                    {
                        throw new Exception($"User specified is invalid!");
                    }
                    this.User = userResponse.entity;
                }
            }

            //optional
            if (roleId > 0)
            {
                var roleResponse = await this.GetRoleAsync(authToken, subscriberId, userId, roleId);
                if (roleResponse == null || !roleResponse.succeeded || roleResponse.entity == null)
                {
                    throw new Exception($"Role specified is invalid!");
                }
                this.Role = roleResponse.entity;
            }

            //optional
            if (jobFunctionId > 0)
            {
                var jobFunctionResponse = await this.GetJobFunctionAsync(authToken, subscriberId, userId, jobFunctionId);
                if (jobFunctionResponse == null || !jobFunctionResponse.succeeded || jobFunctionResponse.entity == null)
                {
                    throw new Exception($"Job function specified is invalid!");
                }
                this.JobFunction = jobFunctionResponse.entity;
            }

            return true;
        }

        public async Task<bool> ValidateRole(string authToken, int subscriberId, string userId, int roleId)
        {
            if (roleId > 0)
            {
                var roleResponse = await this.GetRoleAsync(authToken, subscriberId, userId, roleId);
                if (roleResponse == null || !roleResponse.succeeded || roleResponse.entity == null)
                {
                    throw new Exception($"Role specified is invalid!");
                }
                this.Role = roleResponse.entity;
                return true;
            }
            else
            {
                throw new Exception($"Invalid role specified!");
            }
        }

        public async Task<bool> ValidateJobFunction(string authToken, int subscriberId, string userId, int jobFunctionId)
        {
            if (jobFunctionId > 0)
            {
                var jobFunctionResponse = await this.GetJobFunctionAsync(authToken, subscriberId, userId, jobFunctionId);
                if (jobFunctionResponse == null || !jobFunctionResponse.succeeded || jobFunctionResponse.entity == null)
                {
                    throw new Exception($"Job function specified is invalid!");
                }
                this.JobFunction = jobFunctionResponse.entity;
                return true;
            }
            else
            {
                throw new Exception($"Invalid Job function specified!");
            }
        }

       

        public async Task<EntityVm<SystemSettingDto>> GetSystemSettingAsync(string authToken, int subscriberId, string userId)
        {
            try
            {
                this.SetAuthToken(authToken);
                var systemSettingsUrl = _configuration["AuthService:GetActiveSystemSettingBySubscriberIdApiUrl"] + subscriberId + "/" + userId;
                var settings = await _apiClient.Get<EntityVm<SystemSettingDto>>(systemSettingsUrl, this.AuthToken?.Split(" ")[1]);
                return settings;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


    }

}
