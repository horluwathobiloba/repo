using RubyReloaded.AuthService.Application.Common.Interfaces;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace RubyReloaded.AuthService.Infrastructure.Utility
{
    public class APIClient : IAPIClient
    {
        public async Task<string> PostAPIUrl(string apiUrl, string apiKey, object requestObject, bool isFormData)
        {

            try
            {
                var restClient = new RestClient(apiUrl);
                RestRequest restRequest = new RestRequest(apiUrl, Method.POST);
                if (!string.IsNullOrEmpty(apiKey))
                    restRequest.AddHeader("Authorization", "Bearer " + apiKey);

                restRequest.AddJsonBody(requestObject);
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                IRestResponse restResponse = await restClient.ExecuteAsync(restRequest);
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
