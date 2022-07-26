using OnyxRevamped.DocumentService.Domain.ViewModels;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OnyxRevamped.WindowsService.Utility
{
    public class APIClient
    {

        public async Task<string> GetBearerToken(string tokenApi, object requestObject)
        {
            try
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
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<string> PostAPIUrl(string apiUrl, string apiKey, object requestObject, bool isFormData)
        {

            try
            {
                var restClient = new RestClient(apiUrl);
                RestRequest restRequest = new RestRequest(apiUrl, Method.POST);
                if (!string.IsNullOrEmpty(apiKey))
                    restRequest.AddHeader("Authorization", "Bearer " + apiKey);

                restRequest.AddJsonBody(requestObject);
                System.Threading.CancellationToken token = new System.Threading.CancellationToken();
                IRestResponse restResponse = await restClient.ExecuteAsync(restRequest, token);
                var responseContent = restResponse.Content;
                return responseContent;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<string> GetAPIUrl(string apiUrl, string apiKey, string referenceId)
        {
            try
            {
                var restClient = new RestClient(apiUrl);
                RestRequest restRequest = new RestRequest(apiUrl, Method.GET);
                if (!string.IsNullOrEmpty(apiKey))
                {
                    restRequest.AddHeader("Authorization", "Bearer " + apiKey);
                }
                IRestResponse restResponse = await restClient.ExecuteAsync(restRequest);
                var responseContent = restResponse.Content;
                return responseContent;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
