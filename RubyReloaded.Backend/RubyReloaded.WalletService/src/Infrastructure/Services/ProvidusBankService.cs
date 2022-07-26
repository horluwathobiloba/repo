using Microsoft.Extensions.Configuration;
using RestSharp;
using RestSharp.Authenticators;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Application.Common.Models.Requests;
using RubyReloaded.WalletService.Application.Common.Models.Response;
using RubyReloaded.WalletService.Application.PaymentIntegrations.Queries.GetProvidusBanks;
using RubyReloaded.WalletService.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Infrastructure.Services
{
    public class ProvidusBankService : IProvidusBankService
    {
        private readonly IAPIClientService _apiClient;
        private readonly IConfiguration _configuration;
        private readonly IRestClient _client;
        private readonly IUtilityService _utilityService;
        public static string ClientId { get; set; }
        public static string XAuthSignature { get; set; }
        //public APIRequestDto APIRequestDto { get; set; }
        public ProvidusBankService(IConfiguration configuration, IAPIClientService aPIClientService, IRestClient client, IUtilityService utilityService)
        {
            _apiClient = aPIClientService;
            _configuration = configuration;
            _client = client;
            _utilityService = utilityService;
            ClientId = _configuration["Providus:Client-Id"];
            XAuthSignature = _configuration["Providus:X-Auth-Signature"];
        }
        public async Task<List<AirtimeVM>> GetAirtimeCategories()
        {
            var apiUrl = _configuration["Providus:GetAirtimeCategories"];
            string clientId = _configuration["Providus:Client-Id"];
            string xAuthSignature = _configuration["Providus:X-Auth-Signature"];
            RestRequest restRequest = new RestRequest(apiUrl, Method.GET);
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            var soln = _client.Get(restRequest);
            var response = await _client.ExecuteAsync<List<GetAirtimeCategoriesResponse>>(restRequest);
            var airtimeVmList = new List<AirtimeVM>();

            foreach (var item in response.Data)
            {
                var airtimevm = new AirtimeVM
                {
                    Name = item.name,
                    Bill_id = item.bill_id
                };
                airtimeVmList.Add(airtimevm);
            }
            return airtimeVmList;
        }

        public async Task<T> GetAirtimePaymentUIMap<T>(int airtimeCategoryId)
        {
            //var apiUrl = _configuration["Providus:GetPaymentUiMap"] + airtimeCategoryId;
            //string clientId = _configuration["Providus:Client-Id"];
            //string xAuthSignature = _configuration["Providus:X-Auth-Signature"];
            //RestRequest restRequest = new RestRequest(apiUrl, Method.GET);
            //ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            // var soln = _client.Get(restRequest);
            var response = await GetBillPaymentFields<T>(airtimeCategoryId);
           // var response = await _client.GetAsync<T>(restRequest);
            return response;
        }

        public async Task<T> GetBillPaymentCategories<T>()
        {
            var apiUrl = _configuration["Providus:GetWebCategories"];
            //ClientId = _configuration["Providus:Client-Id"];
            //XAuthSignature = _configuration["Providus:X-Auth-Signature"];
            RestRequest restRequest = new RestRequest(apiUrl, Method.GET);
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

            APIRequestDto request = new APIRequestDto
            {
                ApiUrl = apiUrl,
                ClientId = _configuration["Providus:Client-Id"],
                XAuthSignature = _configuration["Providus:X-Auth-Signature"]
            };
            var response = await _apiClient.Get<T>(request);
            // var response = await _client.ExecuteAsync<T>(restRequest);
            return response;
        }

        public async Task<T> GetBillPaymentCategoryOptions<T>(int categoryId)
        {
            var apiUrl = _configuration["Providus:GetBillPaymentCategoryOptions"] + categoryId;
            //ClientId = _configuration["Providus:Client-Id"];
            //XAuthSignature = _configuration["Providus:X-Auth-Signature"];
            //RestRequest restRequest = new RestRequest(apiUrl, Method.GET);
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

            APIRequestDto request = new APIRequestDto
            {
                ApiUrl = apiUrl,
                ClientId = _configuration["Providus:Client-Id"],
                XAuthSignature = _configuration["Providus:X-Auth-Signature"]
            };
            var response = await _apiClient.Get<T>(request);
           
            return response;
        }

        public async Task<T> GetBillPaymentFields<T>(int billId)
        {
            var apiUrl = _configuration["Providus:GetPaymentUiMap"] + billId;
            //ClientId = _configuration["Providus:Client-Id"];
            //XAuthSignature = _configuration["Providus:X-Auth-Signature"];
            RestRequest restRequest = new RestRequest(apiUrl, Method.GET);
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            APIRequestDto request = new APIRequestDto
            {
                ApiUrl = apiUrl,
                ClientId = _configuration["Providus:Client-Id"],
                XAuthSignature = _configuration["Providus:X-Auth-Signature"]
            };

            var response = await _apiClient.Get<T>(request);
            return response;
        }

        public async Task<List<AirtimeVM>> GetDataServicesCategories()
        {
            var apiUrl = _configuration["Providus:GetDataServicesCategories"];
            string clientId = _configuration["Providus:Client-Id"];
            string xAuthSignature = _configuration["Providus:X-Auth-Signature"];
            RestRequest restRequest = new RestRequest(apiUrl, Method.GET);
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            var soln = _client.Get(restRequest);
            var response = await _client.ExecuteAsync<List<GetAirtimeCategoriesResponse>>(restRequest);
            var airtimeVmList = new List<AirtimeVM>();

            foreach (var item in response.Data)
            {
                var airtimevm = new AirtimeVM
                {
                    Name = item.name,
                    Bill_id = item.bill_id
                };
                airtimeVmList.Add(airtimevm);
            }
            return airtimeVmList;
        }

        public async Task<T> GetDataServicesPayment<T>(int bill)
        {
            var apiUrl = _configuration["Providus:GetPaymentUiMap"] + bill;
            ClientId = _configuration["Providus:Client-Id"];
            XAuthSignature = _configuration["Providus:X-Auth-Signature"];
            RestRequest restRequest = new RestRequest(apiUrl, Method.GET);
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            // var soln = _client.Get(restRequest);
            var response = await _client.GetAsync<T>(restRequest);
            return response;
        }

        public async Task<List<AirtimeVM>> GetElectricityServiceCategories()
        {
            var apiUrl = _configuration["Providus:GetElectricServiceCategories"];
            string clientId = _configuration["Providus:Client-Id"];
            string xAuthSignature = _configuration["Providus:X-Auth-Signature"];
            RestRequest restRequest = new RestRequest(apiUrl, Method.GET);
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            var soln = _client.Get(restRequest);
            var response = await _client.ExecuteAsync<List<GetAirtimeCategoriesResponse>>(restRequest);
            var airtimeVmList = new List<AirtimeVM>();

            foreach (var item in response.Data)
            {
                var airtimevm = new AirtimeVM
                {
                    Name = item.name,
                    Bill_id = item.bill_id
                };
                airtimeVmList.Add(airtimevm);
            }
            return airtimeVmList;
        }

        public async Task<T> GetElectricityServicePayment<T>(int bill)
        {
            var apiUrl = _configuration["Providus:GetPaymentUiMap"] + bill;
            string clientId = _configuration["Providus:Client-Id"];
            string xAuthSignature = _configuration["Providus:X-Auth-Signature"];
            RestRequest restRequest = new RestRequest(apiUrl, Method.GET);
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            // var soln = _client.Get(restRequest);
            var response = await _client.GetAsync<T>(restRequest);
            return response;
        }

        public async Task<List<ProvidusWebCategory>> GetWebCategories()
        {
            var apiUrl = _configuration["Providus:GetWebCategories"];
            string clientId = _configuration["Providus:Client-Id"];
            string xAuthSignature = _configuration["Providus:X-Auth-Signature"];
            RestRequest restRequest = new RestRequest(apiUrl, Method.GET);
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            // var soln = _client.Get(restRequest);

            var response = await _client.ExecuteAsync<List<ProvidusWebCategory>>(restRequest);
            return response.Data;
        }

        public Task<T> MakePayment<T>()
        {
            throw new NotImplementedException();
        }

        public async Task<ProvidusMakePaymentResponse> MakePayment(ProvidusMakePaymentRequest providusMakePaymentRequest)
        {

            var transactonRef = _utilityService.GenerateTransactionReference();
            var body = new
            {
                billId = providusMakePaymentRequest.billId,
                customerAccountNo = providusMakePaymentRequest.customerAccountNo,
                channel_ref = providusMakePaymentRequest.channel_ref,
                inputs = providusMakePaymentRequest.Inputs,
                transaction_ref = transactonRef
            };
            APIRequestDto request = new APIRequestDto
            {
                ApiUrl = _configuration["Providus:MakePayment"],
                ClientId = _configuration["Providus:Client-Id"],
                XAuthSignature = _configuration["Providus:X-Auth-Signature"],
                Username = _configuration["Providus:userName"],
                Password = _configuration["Providus:password"],
                requestObject = body
            };
            var apiResponse = await _apiClient.Post<ProvidusMakePaymentResponse>(request);
            //ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            //var response = await _client.ExecuteAsync<ProvidusMakePaymentResponse>(restRequest);
            //return response.Data;
            return apiResponse;
        }


        public Task MakePayment()
        {
            throw new NotImplementedException();
        }

        public async Task<ProvidusFundTransferResponse> ProvidusFundTransfer(ProvidusFundTransferRequest providusFundTransferRequest)
        {

            APIRequestDto request = new APIRequestDto
            {
                ApiUrl = _configuration["Providus:ProvidusFundTransfer"],
                ClientId = _configuration["Providus:Client-Id"],
                XAuthSignature = _configuration["Providus:X-Auth-Signature"],
                Username = _configuration["Providus:userName"],
                Password = _configuration["Providus:password"],
                requestObject = providusFundTransferRequest
            };
            var apiResponse = await _apiClient.Post<ProvidusFundTransferResponse>(request);
            return apiResponse;
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

        public async Task<ProvidusValidationResponse> Validate(ProvidusValidationRequest providusValidationRequest,int billId)
        {
            try
            {
             
                var url = $"{billId}/customer";
                APIRequestDto request = new APIRequestDto
                {
                    ApiUrl = _configuration["Providus:Validate"]+url,
                    ClientId = _configuration["Providus:Client-Id"],
                    XAuthSignature = _configuration["Providus:X-Auth-Signature"],
                    Username = _configuration["Providus:userName"],
                    Password = _configuration["Providus:password"],
                    requestObject = providusValidationRequest
                };
                var apiResponse = await _apiClient.Post<ProvidusValidationResponse>(request);
                return apiResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
