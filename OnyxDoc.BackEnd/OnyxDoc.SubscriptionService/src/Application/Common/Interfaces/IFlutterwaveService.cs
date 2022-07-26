using Microsoft.Extensions.Configuration;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.Common.Interfaces
{
    public interface IFlutterwaveService : IPaymentService
    {
        Task<FlutterwaveResponse> CancelPaymentPlan(string paymentPlanId);
        Task<FlutterwavePaymentPlanResponse> CreatePaymentPlan(FlutterwavePaymentPlan request);
        Task<FlutterwaveResponse> InitiatePayment( FlutterwaveRequest request,string paymentHash);
        Task<FlutterwaveResponse> GetPaymentStatus(string transactionId);
    }
}
