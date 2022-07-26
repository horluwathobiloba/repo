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
using OnyxDoc.SubscriptionService.Domain.Entities;

namespace OnyxDoc.SubscriptionService.Infrastructure.Services
{
    public class FlutterwaveService : IFlutterwaveService
    {
        private readonly IBearerTokenService _bearerTokenService;
        private readonly IConfiguration _configuration;
        private readonly IAPIClient _apiClient;

        public FlutterwaveService(IConfiguration configuration, IBearerTokenService bearerTokenService, IAPIClient apiClient)
        {
            _bearerTokenService = bearerTokenService;
            _configuration = configuration;
            _apiClient = apiClient;
        }

        public async Task<FlutterwavePaymentPlanResponse> CreatePaymentPlan(FlutterwavePaymentPlan flutterwavePaymentPlanResponse)
        {
            string url = _configuration["Flutterwave:Url"] + "payment-plans/";
            string key = _configuration["Flutterwave:Key"];
            var apiResult = await _apiClient.Post(url, key, flutterwavePaymentPlanResponse);
            FlutterwavePaymentPlanResponse response = JsonConvert.DeserializeObject<FlutterwavePaymentPlanResponse>(apiResult);
            return response;
        }

        public async Task<FlutterwaveResponse> GetPaymentStatus(string transactionId)
        {
            string url = _configuration["Flutterwave:Url"] + "transactions/" + transactionId + "/verify";
            string key = _configuration["Flutterwave:Key"];
            var apiResult = await _apiClient.Get(url, key);
            FlutterwaveResponse response = JsonConvert.DeserializeObject<FlutterwaveResponse>(apiResult);
            return response;
        }

        public async Task<FlutterwaveResponse> CancelPaymentPlan(string paymentPlanId)
        {
            string url = _configuration["Flutterwave:Url"] + "payment-plans/"+paymentPlanId+"/cancel";
            string key = _configuration["Flutterwave:Key"];
            var apiResult = await _apiClient.Get(url, key);
            FlutterwaveResponse response = JsonConvert.DeserializeObject<FlutterwaveResponse>(apiResult);
            return response;
        }

        public async Task<FlutterwaveResponse> InitiatePayment(FlutterwaveRequest request, string paymentHash)
        {
            try
            {
                string url = _configuration["Flutterwave:Url"] + "payments";
                string key = _configuration["Flutterwave:Key"];
                string redirect_Url = _configuration["Flutterwave:RedirectUrl"]+"/flutterwave?item=" + paymentHash;
                var flutterwaveRequest = new
                {
                    tx_ref = request.Tx_Ref,
                    amount = request.Amount.ToString(),
                    currency = request.Currency,
                    redirect_url = redirect_Url,
                    payment_plan = request.Payment_Plan,
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
               
                var apiResult = await _apiClient.Post(url, key, flutterwaveRequest);
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
