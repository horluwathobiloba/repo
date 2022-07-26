using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Application.Models.Paystack;
using OnyxDoc.SubscriptionService.Domain.ViewModels;
using System;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.Common.Interfaces
{
    public interface IPaystackService : IPaymentService
    {
        Task<PaystackPaymentResponseRaw> InitializePayment(PaymentIntentVm paymentIntentVm);
        Task<PaystackPaymentResponseRaw> VerifyPaymentStatus(string ReferneceNo);

    }

    public interface IPaystackSubscriptionService
    {
        Task<Result> CreateSubscription(CreatePaystackSubscriptionRequest request);
        Task<Result> GenerateUpdateSubscriptionLink(string PaystackSubscriptionCode, string accessToken);
        Task<Result> SendUpdateSubscriptionLink(string paystackSubscriptionCode, string accessToken);
        Task<Result> EnableSubscription(string paystackSubscriptionCode, string emailToken);
        Task<Result> DisableSubscription(string paystackSubscriptionCode, string emailToken);
        Task<Result> DeleteSubscription(string paystackSubscriptionCode, string emailToken);
        Task<Result> GetSubscription(string paystackSubscriptionIdOrCode);

        /// <summary>   
        /// List Prices available on your integration.
        /// /// </summary>
        /// <param name="perPage">Specify how many records you want to retrieve per page.If not specify we use a default value of 50.</param>
        /// <param name="page">Specify exactly what page you want to retrieve.If not specify we use a default value of 1.</param>
        /// <param name="from">Filter by from date</param>
        /// <param name="to"> Filter by to date<</param>
        /// <returns></returns>
        Task<Result> GetSubscriptions(int? perPage = null, int? page = null, DateTime? from = null, DateTime? to = null);
    }

    public interface IPaystackPlanService
    {
        Task<Result> CreatePlan(CreatePaystackPlanRequest product);
        Task<Result> UpdatePlan(UpdatePaystackPlanRequest plan, string accessToken);
        Task<Result> DeletePlan(string paystackPlanCode, string userId, string accessToken);
        Task<Result> GetProduct(string paystackPlanCode);

        /// <summary>   
        /// List Products available on your integration.
        /// /// </summary>
        /// <param name="perPage">Specify how many records you want to retrieve per page. If not specify we use a default value of 50.</param>
        /// <param name="page">Specify exactly what page you want to retrieve.If not specify we use a default value of 1.</param>
        /// <param name="from">Filter by from date</param>
        /// <param name="to"> Filter by to date<</param>
        /// <returns></returns>
        Task<Result> GetPlans(int? perPage = null, int? page = null, DateTime? from = null, DateTime? to = null);
    }

    public interface IPaystackCustomerService
    {
        Task<Result> CreateCustomer(CreatePaystackCustomerRequest customer);
        Task<Result> UpdateCustomer(UpdatePaystackCustomerRequest customer, string accessToken);
        Task<Result> WhitelistCustomer(string paystackCustomerCode, string accessToken);
        Task<Result> BlacklistCustomer(string paystackCustomerCode, string accessToken);
        Task<Result> DeactivateAuthorization(string authorizationCode);

        Task<Result> DeleteCustomer(string paystackCustomerCode, string userId, string accessToken);
        Task<Result> GetCustomer(string email_or_customercode);
        /// <summary>   
        /// List Customers available on your integration.
        /// /// </summary>
        /// <param name="perPage">Specify how many records you want to retrieve per page.If not specify we use a default value of 50.</param>
        /// <param name="page">Specify exactly what page you want to retrieve.If not specify we use a default value of 1.</param>
        /// <param name="from">Filter by from date</param>
        /// <param name="to"> Filter by to date<</param>
        /// <returns></returns>
        Task<Result> GetCustomers(int? perPage = null, int? page = null, DateTime? from = null, DateTime? to = null);
    }



    public interface IPaystackProductService
    {
        Task<Result> CreateProduct(CreatePaystackProductRequest product);
        Task<Result> UpdateProduct(UpdatePaystackProductRequest product, string accessToken);
        Task<Result> DeleteProduct(string paystackProductCode, string userId, string accessToken);
        Task<Result> GetProduct(string paystackProductCode);

        /// <summary>   
        /// List Products available on your integration.
        /// /// </summary>
        /// <param name="perPage">Specify how many records you want to retrieve per page.If not specify we use a default value of 50.</param>
        /// <param name="page">Specify exactly what page you want to retrieve.If not specify we use a default value of 1.</param>
        /// <param name="from">Filter by from date</param>
        /// <param name="to"> Filter by to date<</param>
        /// <returns></returns>
        Task<Result> GetProducts(int? perPage = null, int? page = null, DateTime? from = null, DateTime? to = null);
    }


    public interface IPaystackPriceService
    {
        Task<Result> CreatePrice(CreatePaystackPriceRequest price);
        Task<Result> UpdatePrice(UpdatePaystackPriceRequest price, string accessToken);
        Task<Result> DeletePrice(string paystackPriceCode, string userId, string accessToken);
        Task<Result> GetPrice(string PaystackPriceCode);

        /// <summary>   
        /// List Prices available on your integration.
        /// /// </summary>
        /// <param name="perPage">Specify how many records you want to retrieve per page.If not specify we use a default value of 50.</param>
        /// <param name="page">Specify exactly what page you want to retrieve.If not specify we use a default value of 1.</param>
        /// <param name="from">Filter by from date</param>
        /// <param name="to"> Filter by to date<</param>
        /// <returns></returns>
        Task<Result> GetPrices(int? perPage = null, int? page = null, DateTime? from = null, DateTime? to = null);
    }


}
