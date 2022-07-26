using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Domain.ViewModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using OnyxDoc.SubscriptionService.Application.Models.Paystack;
using MediatR;
using OnyxDoc.SubscriptionService.Application.PGCustomers.Queries;
using OnyxDoc.SubscriptionService.Application.PGProducts.Queries;
using OnyxDoc.SubscriptionService.Domain.Enums;
using OnyxDoc.SubscriptionService.Application.PGPrices.Queries;
using OnyxDoc.SubscriptionService.Application.PGSubscriptions.Queries;

namespace OnyxDoc.SubscriptionService.Infrastructure.Services
{
    public class PaystackService : IPaystackService, IPaystackSubscriptionService, IPaystackCustomerService, IPaystackPriceService, IPaystackProductService
    {

        private readonly IAPIClient _apiClient;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;

        public PaystackService(IAPIClient apiClient, IConfiguration configuration, IMediator mediator)
        {
            _apiClient = apiClient;
            _configuration = configuration;
            _mediator = mediator;
        }

        public async Task<PaystackPaymentResponseRaw> InitializePayment(PaymentIntentVm paymentIntentVm)
        {
            try
            {
                string key = _configuration["Paystack:Key"];
                string url = _configuration["Paystack:Url"] + "transaction/initialize";
                var payObject = new
                {
                    email = paymentIntentVm.Email,
                    amount = paymentIntentVm.SubscriptionAmount.ToString(),
                    currency = paymentIntentVm.CurrencyCode,
                    reference = paymentIntentVm.ClientReferenceNo,
                    callback_url = paymentIntentVm.CallBackUrl
                };
                var apiResult = await _apiClient.JsonPost<PaystackPaymentResponseRaw>(url, key, payObject, true);
                return apiResult; // Result.Success("Paystack Initiate Payment Result :", apiResult);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PaystackPaymentResponseRaw> VerifyPaymentStatus(string ReferneceNo)
        {
            string key = _configuration["Paystack:Key"];
            if (!string.IsNullOrEmpty(ReferneceNo))
            {
                var referenceId = ReferneceNo;
                string apiUrl = _configuration["Paystack:Url"] + "transaction/verify/" + ReferneceNo;
                var response = await _apiClient.Get<PaystackPaymentResponseRaw>(apiUrl, key);
                return response; // Result.Success("Paystack Verify Payment Result:", response);
            }
            throw new Exception("Invalid Client Reference Id"); //Result.Failure("Invalid Client Reference Id");
        }


        #region Paystack Subscription Services

        private async Task<PGSubscriptionDto> GetPGSubscription(GetPGSubscriptionQuery command)
        {
            var result = await _mediator.Send(command);
            var pgSubscription = (PGSubscriptionDto)result.Entity;
            if (pgSubscription == null)
            {
                throw new Exception($"Subscription not found! Error message { result.Message}");
            }
            return pgSubscription;
        }

        private async Task<PGSubscriptionDto> GetPGSubscriptionByPGId(GetPGSubscriptionByPGIdQuery command)
        {
            var result = await _mediator.Send(command);
            var pgSubscription = (PGSubscriptionDto)result.Entity;
            if (pgSubscription == null)
            {
                throw new Exception($"Subscription not found! Error message { result.Message}");
            }
            return pgSubscription;
        }


        private async Task<PGSubscriptionDto> GetPGSubscriptionByPGCode(GetPGSubscriptionByPGCodeQuery command)
        {
            var prodResult = await _mediator.Send(command);
            var pgCustomer = (PGSubscriptionDto)prodResult.Entity;
            if (pgCustomer == null)
            {
                throw new Exception($"Paystack customer not found! Error message { prodResult.Message}");
            }
            return pgCustomer;
        }

        public async Task<Result> CreateSubscription(CreatePaystackSubscriptionRequest request)
        {
            try
            {
                string key = _configuration["Paystack:Key"];
                string url = _configuration["Paystack:Url"] + "initialize";
                var payObject = new
                {
                    customer = request.PaystackCustomerCode,
                    plan = request.PaystackPlanCode
                };
                var apiResult = await _apiClient.JsonPost<PaystackSubscriptionResponse>(url, key, payObject, true);
                return Result.Success("Success", apiResult);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Create subscription failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        public async Task<Result> GenerateUpdateSubscriptionLink(string PaystackSubscriptionCode, string accessToken)
        {
            try
            {
                string key = _configuration["Paystack:Key"];
                string url = _configuration["Paystack:Url"] + $"subscription/{PaystackSubscriptionCode}/manage/link";
                var apiResult = await _apiClient.Get<PaystackSubscriptionLinkResponse>(url, key);

                return Result.Success("Success", apiResult);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Generate update subscription link failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        public async Task<Result> SendUpdateSubscriptionLink(string paystackSubscriptionCode, string accessToken)
        {
            try
            {
                string key = _configuration["Paystack:Key"];

                string url = _configuration["Paystack:Url"] + "subscription/" + paystackSubscriptionCode + "/manage/email";
                var apiResult = await _apiClient.JsonPost<PaystackSubscriptionResponse>(url, key, null, true);
                return Result.Success("Success", apiResult);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Send update subscription link failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        public async Task<Result> EnableSubscription(string subscription_code, string email_token)
        {
            try
            {
                string key = _configuration["Paystack:Key"];
                string url = _configuration["Paystack:Url"] + "subscription/enable";
                var payObject = new
                {
                    code = subscription_code,
                    token = email_token
                };
                var apiResult = await _apiClient.JsonPost<PaystackSubscriptionResponse>(url, key, payObject, true);
                return Result.Success("Success", apiResult);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Enable subscription failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        public async Task<Result> DisableSubscription(string paystackSubscriptionCode, string email_token)
        {
            try
            {
                string key = _configuration["Paystack:Key"];
                string url = _configuration["Paystack:Url"] + "subscription/disable";
                var payObject = new
                {
                    code = paystackSubscriptionCode,
                    token = email_token
                };
                var apiResult = await _apiClient.JsonPost<PaystackSubscriptionResponse>(url, key, payObject, true);
                return Result.Success("Success", apiResult);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Disable subscription failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        public async Task<Result> DeleteSubscription(string paystackSubscriptionCode, string emailToken)
        {
            try
            {
                string key = _configuration["Paystack:Key"];
                string url = _configuration["Paystack:Url"] + "subscription/disable";
                var payObject = new
                {
                    code = paystackSubscriptionCode,
                    token = emailToken
                };
                var apiResult = await _apiClient.JsonPost<PaystackSubscriptionResponse>(url, key, payObject, true);
                return Result.Success("Success", apiResult);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Delete subscription failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        public async Task<Result> GetSubscription(string paystackSubscriptionIdOrCode)
        {
            try
            {
                string key = _configuration["Paystack:Key"];
                string url = _configuration["Paystack:Url"] + "subscription" + $"/{paystackSubscriptionIdOrCode}";
                var apiResult = await _apiClient.Get<PaystackSubscriptionResponse>(url, key);
                return Result.Success("Success", apiResult);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Get subscription failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        public async Task<Result> GetSubscriptions(int? perPage = null, int? page = null, DateTime? from = null, DateTime? to = null)
        {
            try
            {
                string key = _configuration["Paystack:Key"];
                string url = _configuration["Paystack:Url"] + "subscription";
                url += $"/{(perPage.HasValue ? perPage.Value : 50)}";
                url += $"/{(page.HasValue ? page.Value : 1)}";

                //add date filter
                string fromDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now); string toDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(1));
                if (from.HasValue && to.HasValue)
                {
                    if (from.Value <= to.Value)
                    {
                        fromDate = $"/{string.Format("{0:yyyy-MM-dd}", from.Value)}"; toDate = $"/{string.Format("{0:yyyy-MM-dd}", to.Value)}";
                    }
                    url += $"/{fromDate}/{toDate}";
                }

                var apiResult = await _apiClient.Get<PaystackCustomerListResponse>(url, key);
                return Result.Success("Success", apiResult);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Get subscriptions failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        #endregion

        #region Paystack Customer Services

        private async Task<PGCustomerDto> GetPGCustomer(GetPGCustomerQuery command)
        {
            var prodResult = await _mediator.Send(command);
            var pgCustomer = (PGCustomerDto)prodResult.Entity;
            if (pgCustomer == null)
            {
                throw new Exception($"Paystack customer not found! Error message { prodResult.Message}");
            }
            return pgCustomer;
        }

        private async Task<PGCustomerDto> GetPGCustomerByPGId(GetPGCustomerByPGIdQuery command)
        {
            var prodResult = await _mediator.Send(command);
            var pgCustomer = (PGCustomerDto)prodResult.Entity;
            if (pgCustomer == null)
            {
                throw new Exception($"Paystack customer not found! Error message { prodResult.Message}");
            }
            return pgCustomer;
        }

        private async Task<PGCustomerDto> GetPGCustomerByPGCode(GetPGCustomerByPGCodeQuery command)
        {
            var prodResult = await _mediator.Send(command);
            var pgCustomer = (PGCustomerDto)prodResult.Entity;
            if (pgCustomer == null)
            {
                throw new Exception($"Paystack customer not found! Error message { prodResult.Message}");
            }
            return pgCustomer;
        }

        public async Task<Result> CreateCustomer(CreatePaystackCustomerRequest request)
        {
            try
            {
                string key = _configuration["Paystack:Key"];
                string url = _configuration["Paystack:Url"] + "customer";
                var payObject = new
                {
                    email = request.Subscriber.Email,
                    first_name = request.Subscriber.Name,
                    last_name = request.Subscriber.Name,
                    phone = request.Subscriber.PhoneNumber,
                    unlimited = true
                };

                var apiResult = await _apiClient.JsonPost<PaystackCustomerResponse>(url, key, payObject, true);
                return Result.Success("Success", apiResult);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Create paystack customer failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        public async Task<Result> UpdateCustomer(UpdatePaystackCustomerRequest request, string accessToken)
        {
            try
            {
                string key = _configuration["Paystack:Key"];

                var customer = await GetPGCustomer(
                 new GetPGCustomerQuery
                 {
                     AccessToken = accessToken,
                     PaymentGateway = request.PaymentGateway,
                     SubscriberId = request.Subscriber.SubscriberId,
                     UserId = request.UserId
                 }
                 );

                string url = _configuration["Paystack:Url"] + "customer/" + customer.PaymentGatewayCustomerCode;
                var payObject = new
                {
                    email = request.Subscriber.Email,
                    first_name = request.Subscriber.Name,
                    last_name = request.Subscriber.Name,
                    phone = request.Subscriber.PhoneNumber,
                    unlimited = true
                };
                var apiResult = await _apiClient.JsonPost<PaystackCustomerResponse>(url, key, payObject, true);
                return Result.Success("Success", apiResult);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Update paystack customer failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }


        public async Task<Result> WhitelistCustomer(string paystackCustomerCode, string accessToken)
        {
            try
            {
                string key = _configuration["Paystack:Key"];

                string url = _configuration["Paystack:Url"] + "customer/set_risk_action";
                var payObject = new
                {
                    customer = paystackCustomerCode,
                    risk_action = "allow",
                };
                var apiResult = await _apiClient.JsonPost<PaystackCustomerResponse>(url, key, payObject, true);
                return Result.Success("Success", apiResult);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Whitelist customer failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        /// <summary>
        /// Deactivate an authorization when the card needs to be forgotten
        /// </summary>
        /// <param name="authorization_code"></param>
        /// <returns></returns>
        public async Task<Result> DeactivateAuthorization(string authorization_code)
        {
            try
            {
                string key = _configuration["Paystack:Key"];

                string url = _configuration["Paystack:Url"] + "customer/deactivate_authorization";
                var payObject = new
                {
                    authorization_code = authorization_code
                };
                var apiResult = await _apiClient.JsonPost<PaystackCustomerResponse>(url, key, payObject, true);
                return Result.Success("Success", apiResult);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Deactivate authorization failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }


        public async Task<Result> BlacklistCustomer(string paystackCustomerCode, string accessToken)
        {
            try
            {
                string key = _configuration["Paystack:Key"];

                string url = _configuration["Paystack:Url"] + "customer/set_risk_action";
                var payObject = new
                {
                    customer = paystackCustomerCode,
                    risk_action = "deny",
                };
                var apiResult = await _apiClient.JsonPost<PaystackCustomerResponse>(url, key, payObject, true);
                return Result.Success("Success", apiResult);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Blacklist customer failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        public async Task<Result> DeleteCustomer(string paystackCustomerCode, string userId, string accessToken)
        {
            try
            {
                string key = _configuration["Paystack:Key"];
                string url = _configuration["Paystack:Url"] + "customer/" + paystackCustomerCode; // TO DO: this needs to change
                var payObject = new
                {
                    active = false
                };
                var apiResult = await _apiClient.JsonPost<PaystackCustomerResponse>(url, key, payObject, true);
                return Result.Success("Success", apiResult);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Get balance failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        public async Task<Result> GetCustomer(string email_or_customercode)
        {
            try
            {
                string key = _configuration["Paystack:Key"];
                string url = _configuration["Paystack:Url"] + "customer" + $"/{email_or_customercode}";
                var apiResult = await _apiClient.Get<PaystackCustomerResponse>(url, key);
                return Result.Success("Success", apiResult);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Get balance failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        public async Task<Result> GetCustomers(int? perPage = null, int? page = null, DateTime? from = null, DateTime? to = null)
        {
            try
            {
                string key = _configuration["Paystack:Key"];
                string url = _configuration["Paystack:Url"] + "customer";
                url += $"/{(perPage.HasValue ? perPage.Value : 50)}";
                url += $"/{(page.HasValue ? page.Value : 1)}";

                //add date filter
                string fromDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now); string toDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(1));
                if (from.HasValue && to.HasValue)
                {
                    if (from.Value <= to.Value)
                    {
                        fromDate = $"/{string.Format("{0:yyyy-MM-dd}", from.Value)}"; toDate = $"/{string.Format("{0:yyyy-MM-dd}", to.Value)}";
                    }
                    url += $"/{fromDate}/{toDate}";
                }

                var apiResult = await _apiClient.Get<PaystackCustomerListResponse>(url, key);
                return Result.Success("Success", apiResult);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Get balance failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        #endregion

        #region Paystack Product Services

        private async Task<PGProductDto> GetPGProduct(GetPGProductQuery command)
        {
            var prodResult = await _mediator.Send(command);
            var pgProduct = (PGProductDto)prodResult.Entity;
            if (pgProduct == null)
            {
                throw new Exception($"Product not found! Error message { prodResult.Message}");
            }
            return pgProduct;
        }

        private async Task<PGProductDto> GetPGProductByPGId(GetPGProductByPGIdQuery command)
        {
            var prodResult = await _mediator.Send(command);
            var pgProduct = (PGProductDto)prodResult.Entity;
            if (pgProduct == null)
            {
                throw new Exception($"Product not found! Error message { prodResult.Message}");
            }
            return pgProduct;
        }

        private async Task<PGProductDto> GetPGProductByPGCode(GetPGProductByPGCodeQuery command)
        {
            var prodResult = await _mediator.Send(command);
            var pgProduct = (PGProductDto)prodResult.Entity;
            if (pgProduct == null)
            {
                throw new Exception($"Product not found! Error message { prodResult.Message}");
            }
            return pgProduct;
        }

        public async Task<Result> CreateProduct(CreatePaystackProductRequest request)
        {
            try
            {
                string key = _configuration["Paystack:Key"];
                string url = _configuration["Paystack:Url"] + "product";
                var payObject = new
                {
                    name = request.Plan.Name,
                    description = request.Plan.Description,
                    price = request.Price.Amount * 100,
                    currency = request.Price.CurrencyCode,
                    unlimited = true
                };
                var apiResult = await _apiClient.JsonPost<PaystackProductResponse>(url, key, payObject, true);
                return Result.Success("Success", apiResult);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Get balance failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        public async Task<Result> UpdateProduct(UpdatePaystackProductRequest request, string accessToken)
        {
            try
            {
                string key = _configuration["Paystack:Key"];

                var product = await GetPGProduct(
                 new GetPGProductQuery
                 {
                     AccessToken = accessToken,
                     PaymentGateway = request.PaymentGateway,
                     SubscriberId = request.Plan.SubscriberId,
                     SubscriptionPlanId = request.Plan.Id,
                     UserId = request.UserId
                 }
                 );

                string url = _configuration["Paystack:Url"] + "product/" + product.PaymentGatewayProductCode; // TO DO: this needs to change
                var payObject = new
                {
                    name = request.Plan.Name + "-" + request.Price.CurrencyCode,
                    description = request.Plan.Description,
                    price = request.Price.Amount * 100,
                    currency = request.Price.CurrencyCode,
                    unlimited = true
                };
                var apiResult = await _apiClient.JsonPost<PaystackProductResponse>(url, key, payObject, true);
                return Result.Success("Success", apiResult);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Get balance failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        public async Task<Result> DeleteProduct(string PaymentProductCode, string userId, string accessToken)
        {
            try
            {
                string key = _configuration["Paystack:Key"];

                var product = await GetPGProductByPGId(
                 new GetPGProductByPGIdQuery
                 {
                     AccessToken = accessToken,
                     PaymentGatewayProductId = PaymentProductCode,
                     UserId = userId
                 }
                 );

                string url = _configuration["Paystack:Url"] + "product/" + product.PaymentGatewayProductCode; // TO DO: this needs to change
                var payObject = new
                {
                    active = false
                };
                var apiResult = await _apiClient.JsonPost<PaystackProductResponse>(url, key, payObject, true);
                return Result.Success("Success", apiResult);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Delete product failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        public async Task<Result> GetProduct(string PaystackProductCode)
        {
            try
            {
                string key = _configuration["Paystack:Key"];
                string url = _configuration["Paystack:Url"] + "product" + $"/{PaystackProductCode}";
                var apiResult = await _apiClient.Get<PaystackProductResponse>(url, key);
                return Result.Success("Success", apiResult);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Get product failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        public async Task<Result> GetProducts(int? perPage = null, int? page = null, DateTime? from = null, DateTime? to = null)
        {
            try
            {
                string key = _configuration["Paystack:Key"];
                string url = _configuration["Paystack:Url"] + "product";
                url += $"/{(perPage.HasValue ? perPage.Value : 50)}";
                url += $"/{(page.HasValue ? page.Value : 1)}";

                //add date filter
                string fromDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now); string toDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(1));
                if (from.HasValue && to.HasValue)
                {
                    if (from.Value <= to.Value)
                    {
                        fromDate = $"/{string.Format("{0:yyyy-MM-dd}", from.Value)}"; toDate = $"/{string.Format("{0:yyyy-MM-dd}", to.Value)}";
                    }
                    url += $"/{fromDate}/{toDate}";
                }

                var apiResult = await _apiClient.Get<PaystackProductListResponse>(url, key);
                return Result.Success("Success", apiResult);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Get products failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        #endregion


        #region Paystack Price Services


        private async Task<PGPriceDto> GetPGPrice(GetPGPriceQuery command)
        {
            var prodResult = await _mediator.Send(command);
            var pgPrice = (PGPriceDto)prodResult.Entity;
            if (pgPrice == null)
            {
                throw new Exception($"Price not found! Error message { prodResult.Message}");
            }
            return pgPrice;
        }

        private async Task<PGPriceDto> GetPGPriceByPGId(GetPGPriceByPGIdQuery command)
        {
            var prodResult = await _mediator.Send(command);
            var pgPrice = (PGPriceDto)prodResult.Entity;
            if (pgPrice == null)
            {
                throw new Exception($"Price not found! Error message { prodResult.Message}");
            }
            return pgPrice;
        }

        private async Task<PGPriceDto> GetPGPriceByPGCode(GetPGPriceByPGCodeQuery command)
        {
            var prodResult = await _mediator.Send(command);
            var pgPrice = (PGPriceDto)prodResult.Entity;
            if (pgPrice == null)
            {
                throw new Exception($"Price not found! Error message { prodResult.Message}");
            }
            return pgPrice;
        }

        public async Task<Result> CreatePrice(CreatePaystackPriceRequest request)
        {
            try
            {
                string key = _configuration["Paystack:Key"];
                string url = _configuration["Paystack:Url"] + "price";
                var payObject = new
                {
                    name = request.Price.Name,
                    description = request.Price.Description,
                    price = request.Price.Amount * 100,
                    currency = request.Price.CurrencyCode,
                    unlimited = true
                };
                var apiResult = await _apiClient.JsonPost<PaystackPriceResponse>(url, key, payObject, true);
                return Result.Success("Success", apiResult);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Create price failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        public async Task<Result> UpdatePrice(UpdatePaystackPriceRequest request, string accessToken)
        {
            try
            {
                string key = _configuration["Paystack:Key"];

                var Price = await GetPGPrice(
                 new GetPGPriceQuery
                 {
                     AccessToken = accessToken,
                     PaymentGateway = request.PaymentGateway,
                     SubscriberId = request.Price.SubscriberId,
                     SubscriptionPlanPricingId = request.Price.Id,
                     UserId = request.UserId
                 }
                 );

                string url = _configuration["Paystack:Url"] + "price/" + Price.PaymentGatewayPriceCode; // TO DO: this needs to change
                var payObject = new
                {
                    name = request.Price.Name + "-" + request.Price.CurrencyCode,
                    description = request.Price.Description,
                    price = request.Price.Amount * 100,
                    currency = request.Price.CurrencyCode,
                    unlimited = true
                };
                var apiResult = await _apiClient.JsonPost<PaystackPriceResponse>(url, key, payObject, true);
                return Result.Success("Success", apiResult);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Get price failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        public async Task<Result> DeletePrice(string paystackPriceCode, string userId, string accessToken)
        {
            try
            {
                string key = _configuration["Paystack:Key"];

                var Price = await GetPGPriceByPGCode(
                 new GetPGPriceByPGCodeQuery
                 {
                     AccessToken = accessToken,
                     PaymentGatewayPriceCode = paystackPriceCode,
                     UserId = userId
                 }
                 );

                string url = _configuration["Paystack:Url"] + "price/" + Price.PaymentGatewayPriceCode; // TO DO: this needs to change
                var payObject = new
                {
                    active = false
                };
                var apiResult = await _apiClient.JsonPost<PaystackPriceResponse>(url, key, payObject, true);
                return Result.Success("Success", apiResult);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Delete paystack pprice failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        public async Task<Result> GetPrice(string paystackPriceCode)
        {
            try
            {
                string key = _configuration["Paystack:Key"];
                string url = _configuration["Paystack:Url"] + "price" + $"/{paystackPriceCode}";
                var apiResult = await _apiClient.Get<PaystackPriceResponse>(url, key);
                return Result.Success("Success", apiResult);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Get price failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }

        public async Task<Result> GetPrices(int? perPage = null, int? page = null, DateTime? from = null, DateTime? to = null)
        {
            try
            {
                string key = _configuration["Paystack:Key"];
                string url = _configuration["Paystack:Url"] + "price";
                url += $"/{(perPage.HasValue ? perPage.Value : 50)}";
                url += $"/{(page.HasValue ? page.Value : 1)}";

                //add date filter
                string fromDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now); string toDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(1));
                if (from.HasValue && to.HasValue)
                {
                    if (from.Value <= to.Value)
                    {
                        fromDate = $"/{string.Format("{0:yyyy-MM-dd}", from.Value)}"; toDate = $"/{string.Format("{0:yyyy-MM-dd}", to.Value)}";
                    }
                    url += $"/{fromDate}/{toDate}";
                }

                var apiResult = await _apiClient.Get<PaystackPriceListResponse>(url, key);
                return Result.Success("Success", apiResult);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Get prices failed! Error: {ex?.Message ?? ex?.InnerException.Message}");
            }
        }


        #endregion



    }
}
