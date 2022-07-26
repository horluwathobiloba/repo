using Microsoft.Extensions.Configuration;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Domain.ViewModels;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OnyxDoc.FormBuilderService.Infrastructure.Services
{
    public class BearerTokenService : IBearerTokenService
    {
        private readonly IConfiguration _configuration;

        public BearerTokenService(IConfiguration configuration)
        {
            _configuration = configuration; 
        }
        public async Task<string> GetBearerToken()
        {
            string appName = _configuration["AuthService:appName"];
            string tokenApiUrl = _configuration["AuthService:tokenApiUrl"];

            IRestClient tokenRestClient = new RestClient();
            IRestRequest tokenRequest = new RestRequest(tokenApiUrl, Method.POST);
            tokenRequest.AddJsonBody(appName);
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            var tokenResp = await tokenRestClient.PostAsync<TokenVm>(tokenRequest);
            IRestResponse restResponse = await tokenRestClient.ExecuteAsync(tokenRequest);
            if (tokenResp != null)
            {
                return tokenResp.Token.AccessToken;
            }
            var responseContent = restResponse.Content;
            return responseContent;
        }

        public async Task<string> GetBearerToken(string tokenApi, object requestObject)
        { 
            IRestClient tokenRestClient = new RestClient();
            IRestRequest tokenRequest = new RestRequest(tokenApi, Method.POST);
            tokenRequest.AddJsonBody(requestObject);
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            var tokenResp = await tokenRestClient.PostAsync<TokenVm>(tokenRequest);
            IRestResponse restResponse = await tokenRestClient.ExecuteAsync(tokenRequest);
            if (tokenResp != null)
            {
                return tokenResp.Token.AccessToken;
            }
            var responseContent = restResponse.Content;
            return responseContent;
        }
    }
}
