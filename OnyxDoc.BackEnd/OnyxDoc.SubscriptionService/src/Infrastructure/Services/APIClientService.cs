using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using OnyxDoc.SubscriptionService.Domain.ViewModels;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OnyxDoc.SubscriptionService.Infrastructure.Services
{
    public class APIClient : IAPIClient
    {
        private readonly IBearerTokenService _bearerTokenService;
        private readonly IConfiguration _configuration;
        private readonly IRestClient _client;

        public APIClient(IConfiguration configuration, IBearerTokenService bearerTokenService, IRestClient client)
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

        public async Task<T> JsonGet<T>(string apiUrl, string apiKey, bool isFormData = false)
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

        public string Serialize<T>(T obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            MemoryStream ms = new MemoryStream();
            serializer.WriteObject(ms, obj);
            string retVal = Encoding.UTF8.GetString(ms.ToArray());
            return retVal;
        }

        public T Deserialize<T>(string json)
        {
            T obj = Activator.CreateInstance<T>();
            MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            obj = (T)serializer.ReadObject(ms);
            ms.Close();
            return obj;
        }

        public async Task<string> Get(string apiUrl, string apiKey, bool isFormData = false)
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

        public async Task<T> Post<T>(string apiUrl, string apiKey, object requestObject, bool isFormData = false)
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
                IRestResponse restResponse = await client.ExecuteAsync<T>(restRequest);
                var responseContent = restResponse.Content;
                return Deserialize<T>(responseContent);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Use JsonConvert to Serialize and Deserialize
        public string JsonSerialize<T>(T obj)
        {
            string jsonString = JsonConvert.SerializeObject(obj.GetType());
            return jsonString;
        }

        public T JsonDeserialize<T>(string json)
        {
            T obj = Activator.CreateInstance<T>();           
            obj = JsonConvert.DeserializeObject<T>(json);
            return obj;
        }

        public async Task<T> JsonPost<T>(string apiUrl, string apiKey, object requestObject, bool isFormData = false)
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
                IRestResponse restResponse = await client.ExecuteAsync<T>(restRequest);
                var responseContent = restResponse.Content;
                return JsonDeserialize<T>(responseContent);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


        public async Task<string> PostAPIUrl(string apiUrl, string apiKey, object requestObject, bool isFormData)
        {

            var restClient = new RestClient(apiUrl);
            RestRequest restRequest = new RestRequest(apiUrl, Method.POST);
            if (!string.IsNullOrEmpty(apiKey))
                restRequest.AddHeader("Authorization", "Bearer " + apiKey);

            restRequest.AddJsonBody(requestObject);
            IRestResponse restResponse = await restClient.ExecuteAsync(restRequest);
            var responseContent = restResponse.Content;
            return responseContent;
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
