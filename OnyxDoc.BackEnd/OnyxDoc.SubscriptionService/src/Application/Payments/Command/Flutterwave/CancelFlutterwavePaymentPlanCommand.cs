using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Application.PaymentChannels.Queries;
using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Domain.Enums;
using OnyxDoc.SubscriptionService.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace OnyxDoc.SubscriptionService.Application.Payments.Commands
{
    public class CancelFlutterwavePaymentPlanCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string CurrencyCode { get; set; }
        public int SubscriptionPlanId { get; set; }
        public string UserId { get; set; }

    }

    public class CancelFlutterwavePaymentPlanCommandHandler : IRequestHandler<CancelFlutterwavePaymentPlanCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;
        private readonly IFlutterwaveService _flutterwaveService;

        public CancelFlutterwavePaymentPlanCommandHandler(IApplicationDbContext context, IAuthService authService, IFlutterwaveService flutterwaveService)
        {
            _context = context;
            _authService = authService;
            _flutterwaveService = flutterwaveService;
        }

        public async Task<Result> Handle(CancelFlutterwavePaymentPlanCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);
              
                var flutterwavePaymentPlan = await _context.FlutterwavePaymentPlans.Where(a =>a.SubscriberId == request.SubscriberId && 
                                             a.SubscriptionPlanId == request.SubscriptionPlanId && a.CurrencyCode == request.CurrencyCode && a.Status == Status.Active).FirstOrDefaultAsync();

                if (flutterwavePaymentPlan== null)
                {
                    return Result.Failure("Invalid flutterwave plan specified.");
                }
                var response = await _flutterwaveService.CancelPaymentPlan(flutterwavePaymentPlan.PaymentPlanId.ToString());
                if (response.status != "success")
                {
                    return Result.Failure("Flutterwave payment plan cancellation is " + response.status + " with message " + response.message);
                }
                
                _context.FlutterwavePaymentPlans.Remove(flutterwavePaymentPlan);
                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success("");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Flutterwave Plan Payment Cancellation failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }
    }
}
