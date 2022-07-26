using Onyx.ContractService.Application.Common.Interfaces;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using Onyx.ContractService.Application.Common.Models;
using Microsoft.Extensions.Configuration;
using System.Net;
using Onyx.ContractService.Domain.Entities;
using System.Collections.Generic;
using Onyx.ContractService.Domain.ViewModels;
using System.Runtime.InteropServices;
using Onyx.ContractService.Infrastructure.Utility;
using System.Linq;

namespace Onyx.ContractService.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IBearerTokenService _bearerTokenService;
        private readonly IConfiguration _configuration;
        private readonly IRestClient _client;
        private readonly IAPIClientService _apiClient;

        public OrganisationDto Organisation { get; set; }
        public JobFunctionDto JobFunction { get; set; }
        public RoleDto Role { get; set; }
        public UserDto User { get; set; }
        public List<RoleDto> Roles { get; set; }
        public List<UserDto> Users { get; set; }
        public List<JobFunctionDto> JobFunctions { get; set; }

        public string JobFunctionId { get; set; }
        public string UserId { get; set; }
        public string AuthToken { get; set; }


        public AuthService(IConfiguration configuration, IRestClient client, IAPIClientService apiClient)
        {
            _configuration = configuration;
            _client = client;
            _apiClient = apiClient;
        }

        public async Task<Tuple<OrganisationVm, RoleListVm, UserListVm, JobFunctionListVm>> GetOrganisationDataAsync(string authToken, int orgId)
        {
            try
            {
                this.SetAuthToken(authToken);

                var orgApiUrl = _configuration["AuthService:GetOrganisationApiUrl"] + orgId + "/" + this.UserId;
                var rolesApiUrl = _configuration["AuthService:GetRolesApiUrl"] + orgId + "/" + this.UserId;
                var usersApiUrl = _configuration["AuthService:GetUsersApiUrl"] + orgId + "/" + this.UserId;
                var jobFunctionsApiUrl = _configuration["AuthService:GetJobFunctionsApiUrl"] + orgId + "/" + this.JobFunctionId;

                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                var org = await _apiClient.Get<OrganisationVm>(orgApiUrl, this.AuthToken);
                var roles = await _apiClient.Get<RoleListVm>(rolesApiUrl, this.AuthToken);
                var users = await _apiClient.Get<UserListVm>(usersApiUrl, this.AuthToken);
                var jobFunctions = await _apiClient.Get<JobFunctionListVm>(jobFunctionsApiUrl, this.AuthToken);

                var result = new Tuple<OrganisationVm, RoleListVm, UserListVm, JobFunctionListVm>(org, roles, users, jobFunctions);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<OrganisationVm> GetOrganisationAsync(string authToken, int orgId)
        {
            try
            {
                this.SetAuthToken(authToken);
                var orgApiUrl = _configuration["AuthService:GetOrganisationApiUrl"] + orgId + "/" + this.UserId;
                var org = await _apiClient.Get<OrganisationVm>(orgApiUrl, this.AuthToken?.Split(" ")[1]);
                return org;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<RoleListVm> GetRolesAsync(string authToken, int orgId)
        {
            try
            {
                this.SetAuthToken(authToken);
                var rolesApiUrl = _configuration["AuthService:GetRolesApiUrl"] + orgId + "/" + this.UserId;
                var roles = await _apiClient.Get<RoleListVm>(rolesApiUrl, this.AuthToken?.Split(" ")[1]);
                this.Roles = roles.Entity;
                return roles;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<JobFunctionListVm> GetJobFubctionsAsync(string authToken, int orgId)
        {
            try
            {
                this.SetAuthToken(authToken);
                var rolesApiUrl = _configuration["AuthService:GetJobFunctionsApiUrl"] + orgId + "/" + this.UserId;
                var jobFunctions = await _apiClient.Get<JobFunctionListVm>(rolesApiUrl, this.AuthToken?.Split(" ")[1]);
                this.JobFunctions = jobFunctions.Entity;
                return jobFunctions;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<JobFunctionVm> GetJobFunctionAsync(string authToken, int jobFunctionId)
        {
            try
            {
                this.SetAuthToken(authToken);
                var roleApiUrl = _configuration["AuthService:GetJobFunctionApiUrl"] + jobFunctionId + "/" + this.UserId;
                var jobFunction = await _apiClient.Get<JobFunctionVm>(roleApiUrl, this.AuthToken?.Split(" ")[1]);
                return jobFunction;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UserListVm> GetUsersAsync(string authToken, int orgId)
        {
            try
            {
                this.SetAuthToken(authToken);
                var usersApiUrl = _configuration["AuthService:GetUsersApiUrl"] + orgId + "/" + this.UserId;
                var users = await _apiClient.Get<UserListVm>(usersApiUrl, this.AuthToken?.Split(" ")[1]);
                this.Users = users.Entity;
                return users;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UserListVm> GetAllUsersAsync()
        {
            try
            {
                var usersApiUrl = _configuration["AuthService:GetAllUsers"];
                var users = await _apiClient.Get<UserListVm>(usersApiUrl,"");
                return users;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<RoleVm> GetRoleAsync(string authToken, int roleId)
        {
            try
            {
                this.SetAuthToken(authToken);
                var roleApiUrl = _configuration["AuthService:GetRoleApiUrl"] + roleId + "/" + this.UserId;
                var role = await _apiClient.Get<RoleVm>(roleApiUrl, this.AuthToken?.Split(" ")[1]);
                return role;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UserVm> GetUserAsync(string authToken, string userId)
        {
            try
            {
                this.SetAuthToken(authToken);
                var userApiUrl = _configuration["AuthService:GetUserApiUrl"] + userId + "/" + this.UserId;
                var user = await _apiClient.Get<UserVm>(userApiUrl, this.AuthToken?.Split(" ")[1]);
                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> IsValidUser(string authToken, string userId)
        {
            try
            {
                var user = await GetUserAsync(authToken, userId);
                if (user == null || string.IsNullOrEmpty(user.Entity.Id))
                {
                    throw new Exception("Invalid role specified.");
                }
                return !string.IsNullOrEmpty(user.Entity.Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> IsValidRole(string authToken, int roleId)
        {
            try
            {
                var role = await GetRoleAsync(authToken, roleId);
                if (role == null || role.Entity.Id <= 0)
                {
                    throw new Exception("Invalid role specified.");
                }
                this.Role = role.Entity;
                return role != null && role.Entity.Id > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> IsValidJobFunction(string authToken, int jobFunctionId)
        {
            try
            {
                var jobFunction = await GetJobFunctionAsync(authToken, jobFunctionId);
                if (jobFunction == null || jobFunction.Entity.Id <= 0)
                {
                    throw new Exception("Invalid job function specified.");
                }
                this.JobFunction = jobFunction.Entity;
                return jobFunction != null && jobFunction.Entity.Id > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<bool> IsValidOrganisation(string authToken, int orgId)
        {
            try
            {
                var org = await GetOrganisationAsync(authToken, orgId);
                if (org == null || org.Entity.Id <= 0)
                {
                    throw new Exception("Invalid organisation specified.");
                }
                return org != null && org.Entity.Id > 0;
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
                var val = accessToken.Claims.First(claim => claim.Type == "userid").Value;
                this.UserId = val;
            }
        }

        private async Task<string> GetBearerToken()
        {
            string appName = _configuration["AuthService:appName"];
            string tokenApiUrl = _configuration["AuthService:tokenApiUrl"];
            var token = await _bearerTokenService.GetBearerToken(tokenApiUrl, new { name = appName });
            return token;
        }


        public async Task<bool> ValidateOrganisationData(string authToken, int organisationId, [Optional] int roleId, [Optional] int jobFunctionId)
        {
            var orgResponse = organisationId <= 0 ? null : await this.GetOrganisationAsync(authToken, organisationId);

            //mandatory
            if (orgResponse == null || !orgResponse.Succeeded || orgResponse.Entity == null)
            {
                throw new Exception($"Organisation specified is invalid!");
            }
            this.Organisation = orgResponse.Entity;

            //optionally mandatory
            if (!string.IsNullOrEmpty(this.UserId))
            {
                var userResponse = await this.GetUserAsync(authToken, this.UserId);
                if (userResponse == null || !userResponse.Succeeded)
                {
                    throw new Exception($"User specified is invalid!");
                }
                this.User = userResponse.Entity;
            }

            //optional
            if (roleId > 0)
            {
                var roleResponse = await this.GetRoleAsync(authToken, roleId);
                if (roleResponse == null || !roleResponse.Succeeded || roleResponse.Entity == null)
                {
                    throw new Exception($"Role specified is invalid!");
                }
                this.Role = roleResponse.Entity;
            }

            //optional
            if (jobFunctionId > 0)
            {
                var jobFunctionResponse = await this.GetJobFunctionAsync(authToken, jobFunctionId);
                if (jobFunctionResponse == null || !jobFunctionResponse.Succeeded || jobFunctionResponse.Entity == null)
                {
                    throw new Exception($"Job function specified is invalid!");
                }
                this.JobFunction = jobFunctionResponse.Entity;
            }

            return true;
        }

        public async Task<bool> ValidateRole(string authToken, int roleId)
        {
            if (roleId > 0)
            {
                var roleResponse = await this.GetRoleAsync(authToken, roleId);
                if (roleResponse == null || !roleResponse.Succeeded || roleResponse.Entity == null)
                {
                    throw new Exception($"Role specified is invalid!");
                }
                this.Role = roleResponse.Entity;
                return true;
            }
            else
            {
                throw new Exception($"Invalid Role specified!");
            }
        }

        public async Task<bool> ValidateJobFunction(string authToken, int jobFunctionId)
        {
            if (jobFunctionId > 0)
            {
                var jobFunctionResponse = await this.GetJobFunctionAsync(authToken, jobFunctionId);
                if (jobFunctionResponse == null || !jobFunctionResponse.Succeeded || jobFunctionResponse.Entity == null)
                {
                    throw new Exception($"Job function specified is invalid!");
                }
                this.JobFunction = jobFunctionResponse.Entity;
                return true;
            }
            else
            {
                throw new Exception($"Invalid Job function  specified!");
            }
        }

    }

}
