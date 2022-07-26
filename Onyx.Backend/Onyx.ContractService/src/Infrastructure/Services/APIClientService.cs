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
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using Onyx.ContractService.Domain.ViewModels;

namespace Onyx.ContractService.Infrastructure.Services
{
    public class APIClientService : IAPIClientService
    {
        private readonly IBearerTokenService _bearerTokenService;
        private readonly IConfiguration _configuration;
        private readonly IRestClient _client;

        public APIClientService(IConfiguration configuration, IBearerTokenService bearerTokenService, IRestClient client)
        {
            _bearerTokenService = bearerTokenService;
            _configuration = configuration;
            _client = client;
        }

        public async Task<T> Get<T>(string apiUrl, string apiKey, bool isFormData = false)
        {
            try
            {
                RestRequest restRequest = new RestRequest(apiUrl);
                if (!string.IsNullOrEmpty(apiKey))
                {
                    restRequest.AddHeader("Accept", "application/json");
                    restRequest.AddHeader("Authorization", "Bearer " + apiKey);
                }
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                var response = await _client.GetAsync<T>(restRequest); 
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string Serialize<T>(T obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            MemoryStream ms = new MemoryStream();
            serializer.WriteObject(ms, obj);
            string retVal = Encoding.UTF8.GetString(ms.ToArray());
            return retVal;
        }

        public static T Deserialize<T>(string json)
        {
            T obj = Activator.CreateInstance<T>();
            MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            obj = (T)serializer.ReadObject(ms);
            ms.Close();
            return obj;
        }


        public async Task<string> Get(string apiUrl, string apiKey, object requestObject, bool isFormData = false)
        {
            try
            {
                RestRequest restRequest = new RestRequest(apiUrl, Method.GET);
                if (!string.IsNullOrEmpty(apiKey))
                {
                    restRequest.AddHeader("Accept", "application/json");
                    restRequest.AddHeader("Authorization", "Bearer " + apiKey);
                }

                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                IRestResponse restResponse = await _client.ExecuteAsync(restRequest);
                var responseContent = restResponse.Content;
                return responseContent;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> Post(string apiUrl, string apiKey, object requestObject, bool isFormData = false)
        {
            try
            {
                var client = new RestClient(apiUrl);
                RestRequest restRequest = new RestRequest(apiUrl, Method.POST);
                if (!string.IsNullOrEmpty(apiKey))
                {
                    restRequest.AddHeader("Accept", "application/json");
                    restRequest.AddHeader("Authorization", "Bearer " + apiKey);
                }
                restRequest.AddJsonBody(requestObject);
                IRestResponse restResponse = await client.ExecuteAsync(restRequest);
                var responseContent = restResponse.Content;
                return responseContent;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<Tuple<OrganisationVm, RoleListVm, UserListVm>> GetOrganisationData(int orgId)
        {
            try
            {
                var roleApiUrl = _configuration["AuthService:RoleApiUrl"];
                var orgApiUrl = _configuration["AuthService:OrganisationApiUrl"];
                var userApiUrl = _configuration["AuthService:UserApiUrl"];

                var token = await this.GetToken();

                IRestClient client = new RestClient();

                //Load the organisation
                IRestRequest orgRequest = new RestRequest(orgApiUrl + orgId);
                orgRequest.AddHeader("Accept", "application/json");
                orgRequest.AddHeader("authorization", "Bearer " + token);

                //Load the roles
                IRestRequest roleRequest = new RestRequest(roleApiUrl + orgId);
                roleRequest.AddHeader("Accept", "application/json");
                roleRequest.AddHeader("authorization", "Bearer " + token);

                //Load the users
                IRestRequest usersRequest = new RestRequest(userApiUrl + orgId);
                usersRequest.AddHeader("Accept", "application/json");
                usersRequest.AddHeader("authorization", "Bearer " + token);

                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                var orgResponse = client.Get<OrganisationVm>(orgRequest);
                var rolesResponse = client.Get<RoleListVm> (roleRequest);
                var usersResponse = client.Get<UserListVm>(usersRequest);

                var result = new Tuple<OrganisationVm, RoleListVm, UserListVm>(orgResponse.Data, rolesResponse.Data, usersResponse.Data);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Tuple<OrganisationVm, List<RoleVm>, List<UserVm>>> LoadOrganisationData(int orgId)
        {
            try
            {
                var orgApiUrl = _configuration["AuthService:GetOrganisationApiUrl"];
                var rolesApiUrl = _configuration["AuthService:GetRolesApiUrl"];
                var usersApiUrl = _configuration["AuthService:GetUsersApiUrl"];

                var token = await this.GetToken();

                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                var org = await this.Get<OrganisationVm>(orgApiUrl + orgId, token);
                var roles = await this.Get<List<RoleVm>>(rolesApiUrl + orgId, token);
                var users = await this.Get<List<UserVm>>(usersApiUrl + orgId, token);

                var result = new Tuple<OrganisationVm, List<RoleVm>, List<UserVm>>(org, roles, users);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<string> GetToken()
        {
            string appName = _configuration["AuthService:appName"];
            string tokenApiUrl = _configuration["AuthService:tokenApiUrl"];
            var token = await _bearerTokenService.GetBearerToken(tokenApiUrl, new { name = appName });
            return token;
        }
    }
}
