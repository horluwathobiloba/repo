using Microsoft.Extensions.Configuration;
using RestSharp;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models.Response;
using RubyReloaded.WalletService.Domain.ViewModels;
using RubyReloaded.WalletService.Infrastructure.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IBearerTokenService _bearerTokenService;
        private readonly IConfiguration _configuration;
        private readonly IRestClient _client;
        private readonly IAPIClientService _apiClient;

        public string UserId { get; set; }
        public List<UserDto> Users { get; set; }
        public UserDto User { get; set; }
        public string AuthToken { get; set; }
        public APIRequestDto aPIRequestDto { get; set; }
        public AuthService(IConfiguration configuration, IRestClient client, IAPIClientService apiClient,IBearerTokenService bearerTokenService)
        {
            _configuration = configuration;
            _client = client;
            _apiClient = apiClient;
            _bearerTokenService = bearerTokenService;
        }

        public async Task<EntityVm<List<UserDto>>> GetUsersAsync(string authToken, int subscriberId, string userId, int skip = 0, int take = 0)
        {
            try
            {
                this.SetAuthToken(authToken);
                var usersApiUrl = _configuration["AuthService:GetUsersApiUrl"] + subscriberId + "/" + userId + "/" + skip + "/" + take;
                aPIRequestDto.ApiUrl = usersApiUrl;
                aPIRequestDto.ApiKey = this.AuthToken?.Split(" ")[1];
                var users = await _apiClient.Get<EntityVm<List<UserDto>>>(aPIRequestDto);
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
                aPIRequestDto.ApiUrl = usersApiUrl;
                aPIRequestDto.ApiKey = this.AuthToken?.Split(" ")[1];
                var users = await _apiClient.Get<EntityVm<List<UserDto>>>(aPIRequestDto);
                return users;
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
                aPIRequestDto.ApiUrl = userApiUrl;
                aPIRequestDto.ApiKey = this.AuthToken?.Split(" ")[1];
                var user = await _apiClient.Get<EntityVm<UserDto>>(aPIRequestDto);
                return user;
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
                this.UserId = userId;
            }
        }

        protected async Task<string> GetBearerToken()
        {
            string appName = _configuration["AuthService:appName"];
            string tokenApiUrl = _configuration["AuthService:tokenApiUrl"];
            var token = await _bearerTokenService.GetBearerToken(tokenApiUrl, new { name = appName });
            return token;
        }

        public async Task<GetUserByIdResponse> GetUserById(string authtoken, string userId)
        {
         
            this.SetAuthToken(authtoken);
            var userApiUrl = _configuration["AuthService:GetUserApiUrl"] + userId + "/" + this.UserId;
            aPIRequestDto.ApiUrl = userApiUrl;
            aPIRequestDto.ApiKey = this.AuthToken?.Split(" ")[1];
            var user = await _apiClient.Get<GetUserByIdResponse>(aPIRequestDto);
            return user;
        }

    }

}
