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
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace OnyxDoc.SubscriptionService.Application.Payments.Commands
{
    public class CreateFlutterwavePaymentPlanCommand : AuthToken, IRequest<Result>
    {
        public decimal Amount { get; set; }
        public string Interval { get; set; }
        public string Duration { get; set; }
        public int SubscriberId { get; set; }
        public int SubscriptionPlanId { get; set; }
        public string UserId { get; set; }

    }

    public class CreateFlutterwavePaymentPlanCommandHandler : IRequestHandler<CreateFlutterwavePaymentPlanCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;
        private readonly IFlutterwaveService _flutterwaveService;

        public CreateFlutterwavePaymentPlanCommandHandler(IApplicationDbContext context, IAuthService authService, IFlutterwaveService flutterwaveService)
        {
            _context = context;
            _authService = authService;
            _flutterwaveService = flutterwaveService;
        }

        public async Task<Result> Handle(CreateFlutterwavePaymentPlanCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);
              
                var subscriptionPlan = await _context.SubscriptionPlans.FirstOrDefaultAsync(a => a.SubscriberId == request.SubscriberId && a.Id == request.SubscriptionPlanId);

                if (subscriptionPlan == null)
                {
                    return Result.Failure("Invalid subscription plan specified.");
                }
                FlutterwavePaymentPlan paymentPlan = new FlutterwavePaymentPlan
                {
                    Amount = request.Amount,
                    Duration = request.Duration,
                    Interval = request.Interval,
                    Name = subscriptionPlan.Name
                };
               var response = await  _flutterwaveService.CreatePaymentPlan(paymentPlan);
                if (response.status != "success")
                {
                    return Result.Failure("Flutterwave payment plan creation is " + response.status + " with message " + response.message);
                }
                //create the plan on our end
                FlutterwavePaymentPlan flutterwavePlan = new FlutterwavePaymentPlan
                {
                    Amount = response.data.amount,
                    Interval = response.data.interval,
                    Duration = response.data.duration.ToString(),
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    PaymentPlanId = response.data.id,
                    SubscriptionPlanId = subscriptionPlan.Id,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString(),
                    SubscriberId = request.SubscriberId,
                    UserId = request.UserId,
                    Name = subscriptionPlan.Name
                };

                 _context.FlutterwavePaymentPlans.Add(flutterwavePlan);
                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success(response);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Flutterwave Plan Payment creation failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }
    }
}
