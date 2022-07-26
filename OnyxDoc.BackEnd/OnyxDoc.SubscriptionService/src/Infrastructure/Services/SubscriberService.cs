using Microsoft.Extensions.Configuration;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Domain.ViewModels;
using OnyxDoc.SubscriptionService.Infrastructure.Utility;
using RestSharp;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Infrastructure.Services
{

    public class SubscriberService : ISubscriberService
    {
        private readonly IBearerTokenService _bearerTokenService;
        private readonly IConfiguration _configuration;
        private readonly IRestClient _client;
        private readonly IAPIClient _apiClient;

        public SubscriberDto Subscriber { get; set; }
        public SubscriberObjectDto SubscriberObject { get; set; }
        public SubscriberAdminDto SubscriberAdmin { get; set; }
        public List<SubscriberDto> Subscribers { get; set; }

        public int SubscriberId { get; set; }
        public string UserId { get; set; }
        public string AuthToken { get; set; }

        public SubscriberService(IConfiguration configuration, IRestClient client, IAPIClient apiClient)
        {
            _configuration = configuration;
            _client = client;
            _apiClient = apiClient;
        }

        public async Task<EntityVm<SubscriberResponseDto>> SignUpSubscriberAsync(string authToken, CreateSubscriberRequest request)
        {
            try
            {
                this.SetAuthToken(authToken);
                var subscriberApiUrl = _configuration["AuthService:SignUpSubscriberUrl"];
                var response = await _apiClient.JsonPost<EntityVm<SubscriberResponseDto>>(subscriberApiUrl, this.AuthToken?.Split(" ")[1], request, true);
                if (response.Succeeded)
                {
                    SubscriberObject = response.Entity.subscriberResponse;
                    SubscriberAdmin = response.Entity.user;
                }
               
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EntityVm<SubscriberDto>> ActivateSubscriberFreeTrialAsync(string authToken, int subcriberId, string userId)
        {
            try
            {
                this.SetAuthToken(authToken);
                dynamic request = new { SubscriberId = subcriberId, UserId = userId };
                var subscriberApiUrl = _configuration["AuthService:ActivateSubscriberFreeTrialUrl"];
                var subscriber = await _apiClient.Post<EntityVm<SubscriberDto>>(subscriberApiUrl, this.AuthToken?.Split(" ")[1], request, true);
                return subscriber;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> CompleteSubscriberFreeTrialAsync(string authToken, int subcriberId, string userId)
        {
            try
            {
                this.SetAuthToken(authToken);
                dynamic request = new { SubscriberId = subcriberId, UserId = userId };
                var subscriberApiUrl = _configuration["AuthService:CompleteSubscriberFreeTrialUrl"];
                var subscriber = await _apiClient.Post<bool>(subscriberApiUrl, this.AuthToken?.Split(" ")[1], request, true);
                return subscriber;
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
                this.UserId = userId;
                this.SubscriberId = subscriberId.ToInt();
            }
        }

    }

}
