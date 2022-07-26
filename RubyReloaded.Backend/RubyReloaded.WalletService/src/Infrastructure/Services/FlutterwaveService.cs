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
    public class FlutterwaveService : IFlutterwaveService
    {
        private readonly IBearerTokenService _bearerTokenService;
        private readonly IConfiguration _configuration;
        private readonly IAPIClientService _apiClient;

        public FlutterwaveService(IConfiguration configuration, IBearerTokenService bearerTokenService, IAPIClientService apiClient)
        {
            _bearerTokenService = bearerTokenService;
            _configuration = configuration;
            _apiClient = apiClient;
        }

        public async Task<FlutterwaveResponse> GetPaymentStatus(string transactionId)
        {
            string url = _configuration["Flutterwave:Url"] + "transactions/" + transactionId + "/verify";
            string key = _configuration["Flutterwave:Key"];
            var getRequest = new APIRequestDto
            {
                ApiUrl = url,
                ApiKey = key
            };
            var apiResult = await _apiClient.Get(getRequest);
            FlutterwaveResponse response = JsonConvert.DeserializeObject<FlutterwaveResponse>(apiResult);
            return response;
        }

        public async Task<FlutterwaveResponse> InitiatePayment(FlutterwaveRequest request, string paymentHash)
        {
            try
            {
                string url = _configuration["Flutterwave:Url"] + "payments";
                string key = _configuration["Flutterwave:Key"];
                string redirect_Url = _configuration["Flutterwave:RedirectUrl"] +"/?item=" + paymentHash;
                var flutterwaveRequest = new
                {
                    tx_ref = request.Tx_Ref,
                    amount = request.Amount.ToString(),
                    currency = request.Currency,
                    redirect_url = redirect_Url,
                    payment_options = "card",
                    meta = new
                    {
                        consumer_id = 0,
                        consumer_mac = "",
                    },
                    customer = new
                    {
                        email = request.Customer.Email,
                        phonenumber = request.Customer.PhoneNumber,
                        name = request.Customer.Name
                    },
                    customizations = new
                    {
                        description = "Simplifying Payments",
                        logo = _configuration["Logo"],
                        title = _configuration["ApplicationName"]
                    }
                };
                var postRequestBody = new APIRequestDto
                {
                    ApiKey=key,
                    ApiUrl=url,
                    requestObject=flutterwaveRequest
                };
                var apiResult = await _apiClient.Post(postRequestBody);
                

                FlutterwaveResponse response = JsonConvert.DeserializeObject<FlutterwaveResponse>(apiResult);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
