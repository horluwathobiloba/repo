using RubyReloaded.SubscriptionService.Application.Common.Interfaces;
using RubyReloaded.SubscriptionService.Domain.ViewModels;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RubyReloaded.SubscriptionService.Infrastructure.Services
{
    public class BearerTokenService : IBearerTokenService
    {
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
