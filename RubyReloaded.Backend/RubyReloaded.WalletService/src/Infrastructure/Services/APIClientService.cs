using RubyReloaded.WalletService.Application.Common.Interfaces;
using System;
using System.Threading.Tasks;
using RestSharp;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using RubyReloaded.WalletService.Domain.ViewModels;
using RubyReloaded.WalletService.Application.Common.Models.Requests;
using RubyReloaded.WalletService.Application.Common.Models.Response;
using RubyReloaded.WalletService.Application.PaymentIntegrations.Queries.GetProvidusBanks;
using RestSharp.Authenticators;

namespace RubyReloaded.WalletService.Infrastructure.Services
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

        public APIClientService()
        {
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
        public async Task<GetAllProvidusBankResponse> GetProvidusBanks()
        {
            try
            {
                string apiUrl = _configuration["Providus:GetNIPBanks"];
                string clientId = _configuration["Providus:Client-Id"];
                string xAuthSignature = _configuration["Providus:X-Auth-Signature"];

                RestRequest restRequest = new RestRequest(apiUrl, Method.GET);
                restRequest.AddHeader("Accept", "application/json");
                restRequest.AddHeader("Client-Id", clientId);
                restRequest.AddHeader("X-Auth-Signature", xAuthSignature);
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                var response = await _client.GetAsync<GetAllProvidusBankResponse>(restRequest);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<GetProvidusAccountResponse> GetWalletBalance(GetProvidusAccountRequest getProvidusAccountRequest)
        {
            try
            {
                string apiUrl = _configuration["Providus:GetProvidusAccount"];
                string clientId = _configuration["Providus:Client-Id"];
                string xAuthSignature = _configuration["Providus:X-Auth-Signature"];
                var client = new RestClient(apiUrl);
                RestRequest restRequest = new RestRequest(apiUrl, Method.POST);
                restRequest.AddHeader("Accept", "application/json");
                restRequest.AddHeader("Client-Id", clientId);
                restRequest.AddHeader("X-Auth-Signature", xAuthSignature);
                restRequest.AddJsonBody(getProvidusAccountRequest);
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                var restResponse = await _client.ExecuteAsync<GetProvidusAccountResponse>(restRequest);

                var responseContent = restResponse.Data;
                return responseContent;

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


        public async Task<string> Get(APIRequestDto request)
        {
            try
            {
                RestRequest restRequest = new RestRequest(request.ApiUrl, Method.GET);
                if (!string.IsNullOrEmpty(request.ApiKey))
                {
                    restRequest.AddHeader("Accept", "application/json");
                    restRequest.AddHeader("Authorization", "Bearer " + request.ApiKey);
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
         
        public async Task<string> Post(APIRequestDto request)
        {
            try
            {
                var client = new RestClient(request.ApiUrl);
                RestRequest restRequest = new RestRequest(request.ApiUrl, Method.POST);
                if (!string.IsNullOrEmpty(request.ApiKey))
                {
                    restRequest.AddHeader("Accept", "application/json");
                    restRequest.AddHeader("Authorization", "Bearer " + request.ApiKey);
                }
                restRequest.AddJsonBody(request.requestObject);
                IRestResponse restResponse = await client.ExecuteAsync(restRequest);
                var responseContent = restResponse.Content;
                return responseContent;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<T> Post<T>(APIRequestDto request)
        {
            try
            {
               // var client = new RestClient(request.ApiUrl);
                RestRequest restRequest = new RestRequest(request.ApiUrl, Method.POST);
                if (!string.IsNullOrEmpty(request.ApiKey))
                {
                    restRequest.AddHeader("Accept", "application/json");
                    restRequest.AddHeader("Authorization", "Bearer " + request.ApiKey);
                }
                if (!string.IsNullOrEmpty(request.Username) && !string.IsNullOrEmpty(request.Password))
                {

                    _client.Authenticator = new HttpBasicAuthenticator(request.Username, request.Password);
                }
                restRequest.AddJsonBody(request.requestObject);
              // IRestResponse restResponse = await _client.ExecuteAsync<T>(restRequest);
               var restResponse = await _client.ExecuteAsync<T>(restRequest);

                //  var response = await _client.ExecuteAsync<T>(restRequest);
                var responseContent = restResponse.Data;
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
        public async Task<DynamicAccountNumberResponseDto> CreateDynamicAccountNumber(string requestObject)
        {
            try
            {
                string apiUrl = _configuration["Providus:DynamicAccountNumber"];
                string clientId = _configuration["Providus:Client-Id"];
                string xAuthSignature = _configuration["Providus:X-Auth-Signature"];
                var client = new RestClient(apiUrl);
                RestRequest restRequest = new RestRequest(apiUrl, Method.POST);

                restRequest.AddHeader("Accept", "application/json");
                restRequest.AddHeader("Client-Id", clientId);
                restRequest.AddHeader("X-Auth-Signature", xAuthSignature);

                
                var body = new
                {
                    account_name = requestObject
                };
                 restRequest.AddJsonBody(body);
                //restRequest.RequestFormat = DataFormat.Json;
              //  restRequest.AddParameter("application/json", body, ParameterType.RequestBody);
               var restResponse = await _client.ExecuteAsync<DynamicAccountNumberResponseDto>(restRequest);
                
                var responseContent = restResponse.Data;
                return responseContent;
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

        public async Task<NipFundTransferResponse> NipFundTransfer(NipFundTransferRequest nipFundTransferRequest)
        {
            try
            {
                string apiUrl = _configuration["Providus:NIPFundTransfer"];
                string clientId = _configuration["Providus:Client-Id"];
                string xAuthSignature = _configuration["Providus:X-Auth-Signature"];

                var client = new RestClient(apiUrl);
                RestRequest restRequest = new RestRequest(apiUrl, Method.POST);

                restRequest.AddHeader("Accept", "application/json");
                restRequest.AddHeader("Client-Id", clientId);
                restRequest.AddHeader("X-Auth-Signature", xAuthSignature);

                restRequest.AddJsonBody(nipFundTransferRequest);
                var restResponse = await _client.ExecuteAsync<NipFundTransferResponse>(restRequest);

                var responseContent = restResponse.Data;
                return responseContent;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<T> Get<T>(APIRequestDto request)
        {
            try
            {
                RestRequest restRequest = new RestRequest(request.ApiUrl, Method.GET);
                if (!string.IsNullOrEmpty(request.ApiKey))
                {
                    restRequest.AddHeader("Accept", "application/json");
                    restRequest.AddHeader("Authorization", "Bearer " + request.ApiKey);
                }
                if (!string.IsNullOrEmpty(request.Username) && !string.IsNullOrEmpty(request.Password))
                {

                    _client.Authenticator = new HttpBasicAuthenticator(request.Username, request.Password);
                }

                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                var restResponse = await _client.ExecuteAsync<T>(restRequest);
                var responseContent = restResponse.Data;
                return responseContent;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
