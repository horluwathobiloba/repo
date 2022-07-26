using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Domain.ViewModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Domain.Enums;
using Anelloh.APIServices.Domain.ViewModels;

namespace OnyxDoc.SubscriptionService.Application.Common.Interfaces
{
    public interface IStripeService : IPaymentService
    {
        Task<Result> InitiateCardPayment(PaymentIntentVm paymentIntentVm);
        Task<Result> GetPaymentStatus(PaymentIntentVm paymentIntentVm);
        //Task<Result> InitiateInstantPayout(IConfiguration _configuration, PayoutVm payoutVm);
        //Task<Result> InitiateManualPayout(IConfiguration _configuration, PayoutVm payoutVm);
        Task<Result> GetBalance();
    }

    public interface IStripeSubscriptionService
    {
        Task<Result> CreateSubscription(CreateStripeSubscriptionRequest request);
        Task<Result> UpdateSubscription(UpdateStripeSubscriptionRequest product);
        Task<Result> CancelSubscription(string stripeSubscriptionId);
        Task<Result> DeleteSubscription(string stripeSubscriptionId);
        Task<Result> GetSubscription(string stripeSubscriptionId);
        Task<Result> GetSubscriptions(string startingAfter = null, string endingBefore = null, long? limit = null);
    }

    public interface IStripeCustomerService
    {
        Task<Result> CreateCustomer(CreateStripeCustomerRequest product);
        Task<Result> UpdateCustomer(UpdateStripeCustomerRequest product);
        Task<Result> DeleteCustomer(string stripeCustomerId);
        Task<Result> GetCustomer(string stripeCustomerId);
        Task<Result> GetCustomers(string startingAfter = null, string endingBefore = null, long? limit = null);
    }

    public interface IStripeProductService
    {
        Task<Result> CreateProduct(CreateStripeProductRequest product);
        Task<Result> UpdateProduct(UpdateStripeProductRequest product);
        Task<Result> DeleteProduct(string stripeProductId);
        Task<Result> GetProduct(string stripeProductId);
        Task<Result> GetProducts(string startingAfter = null, string endingBefore = null, long? limit = null);
    }


    public interface IStripePriceService
    {
        Task<Result> CreatePrice(CreateStripePriceRequest product);
        Task<Result> UpdatePrice(UpdateStripePriceRequest product);
        Task<Result> DeletePrice(string stripePriceId);
        Task<Result> GetPrice(string stripePriceId);
        Task<Result> GetPrices(string startingAfter = null, string endingBefore = null, long? limit = null);
    }

    public interface IPaymentGateway
    {

    }

    public interface IExtra
    {

        Task<Result> CreatePlan(CreateStripePlanRequest plan);
        Task<Result> UpdatePlan(UpdateStripePlanRequest plan);

        //Deleting plans means new subscribers can’t be added. Existing subscribers aren’t affected.
        Task<Result> DeletePlan(string planId);
        Task<StripePlanVm> GetPlan(SubscriptionPlan plan);
        Task<List<StripePlanVm>> GetPlans(SubscriptionPlan plan);



        Task<StripeCustomer> CreateCustomer(string firstname, string lastname, string email, int subscriberId);

        Task<List<StripeCustomer>> GetCustomers(int take);
        Task<StripeCustomer> GetCustomerByCustomerId(string customerId);
        Task<StripeCustomer> GetCustomerByEmail(string email);

        Task<StripeCustomer> DeleteCustomerById(string email);
        Task<StripeCustomer> DeleteCustomerByEmail(string email);
        /// <summary>
        /// when you want to add a payment method for future payment for this particular customer.
        /// Use the return object depending depending the payment provider, e.g. for stripe use the IntentSecret as the ClientSecret
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        Task<FuturePaymentIntentVm> CreateFuturePayment(StripeCustomer customer);
        Task<List<StripePaymentMethodVm>> GetPaymentMethods(StripeCustomer customer, PaymentMethodType paymentMethodType);
        Task<List<StripePaymentMethodVm>> GetPaymentMethodsByCustomerEmail(string customerEmail, PaymentMethodType paymentMethodType);
        Task<StripePaymentMethodVm> AttachPaymentMethod(string paymentMethodId, string customerId, bool makeDefault);

        Task DetachPaymentMethod(string paymentMethodId, string customerId);

        Task<StripeSubscriptionVm> TryCreateSubscription(CreateStripeSubscriptionRequest request);

        /// <summary>
        /// when you want to charge a customer 
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="paymentMethodId"></param>
        /// <returns></returns>
        Task Charge(StripeCustomer customer, StripePaymentMethodVm paymentMethod, Currency currency, long unitAmount, bool sendEmailAfterSuccess = false, string emailDescription = "");

        Task<IEnumerable<Result>> GetPaymentStatus(string paymentId);
    }
}
