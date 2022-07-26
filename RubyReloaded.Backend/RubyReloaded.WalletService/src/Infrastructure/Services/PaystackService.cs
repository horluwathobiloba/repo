using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Infrastructure.Services
{
    public class PaystackService : IPaystackService
    {

        private readonly IAPIClientService _apiClient;
        private readonly IConfiguration _configuration;
       // public  APIRequestDto APIRequestDto;
       // public APIRequestDto APIRequestDto { get; set; }
        public UserDto User { get; set; }

        public PaystackService(IAPIClientService apiClient, IConfiguration configuration)
        {
            _apiClient = apiClient;
            _configuration = configuration;
        }

        public async Task<PaystackPaymentResponse> GetTransactionStatus(PaymentIntentVm paymentIntentVm)
        {
            try
            {
                string key = _configuration["Paystack:Key"];
                if (paymentIntentVm.ClientReferenceId != null)
                {
                    var referenceId = paymentIntentVm.ClientReferenceId;
                    string apiUrl = _configuration["Paystack:Url"] + "transaction/verify/" + referenceId;
                    var aPIRequestDto = new APIRequestDto
                    {
                        ApiKey = key,
                        ApiUrl = apiUrl
                    };
                    var response = await _apiClient.Get<PaystackPaymentResponse>(aPIRequestDto);
                    return response;
                }
                throw new Exception("Invalid Client Reference Id");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<PaystackPaymentResponse> InitializeTransaction(PaymentIntentVm paymentIntentVm)
        {
            try
            {
                string key = _configuration["Paystack:Key"];
                string url = _configuration["Paystack:Url"] + "transaction/initialize";
                var payObject = new
                {
                    email = paymentIntentVm.Email,
                    amount = paymentIntentVm.Amount.ToString(),
                    currency = paymentIntentVm.CurrencyCode,
                    reference = paymentIntentVm.ClientReferenceId,
                    callback_url = paymentIntentVm.CallBackUrl
                };
                var aPIRequestDto = new APIRequestDto
                {
                    ApiKey = key,
                    ApiUrl = url,
                    requestObject = payObject
                };
              
                var apiResult = await _apiClient.Post(aPIRequestDto);
                var result = JsonConvert.DeserializeObject<PaystackPaymentResponse>(apiResult);
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<PaystackPaymentResponse> ChargeAuthorization(PaymentIntentVm cardAuthorization)
        {
            try
            {
                string key = _configuration["Paystack:Key"];
                string url = _configuration["Paystack:Url"] + "transaction/charge_authorization";
                var authorizationRequest = new
                {
                    email = cardAuthorization.Email,
                    amount = cardAuthorization.Amount.ToString(),
                    authorization_code = cardAuthorization.AuthorizationCode
                };
                var aPIRequestDto = new APIRequestDto
                {
                    ApiKey = key,
                    ApiUrl = url,
                    requestObject = authorizationRequest
                };
               var apiResult = await _apiClient.Post(aPIRequestDto);
                var result = JsonConvert.DeserializeObject<PaystackPaymentResponse>(apiResult);
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<PaystackPaymentResponse> DeactivateAuthorization(string authorizationCode)
        {
            try
            {
                string key = _configuration["Paystack:Key"];
                string url = _configuration["Paystack:Url"] + "customer/deactivate_authorization";
                var aPIRequestDto = new APIRequestDto
                {
                    ApiKey = key,
                    ApiUrl = url
                };

                var apiResult = await _apiClient.Post(aPIRequestDto);
                var result = JsonConvert.DeserializeObject<PaystackPaymentResponse>(apiResult);
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
