using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Application.Subscriptions.Queries;
using OnyxDoc.SubscriptionService.Domain.Enums;
using OnyxDoc.SubscriptionService.Domain.ViewModels;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.Subscriptions.Commands
{
    public class UpdateSubscriptionStatusCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int SubscriptionPlanId { get; set; }
        public int Id { get; set; }
        public SubscriptionStatus SubscriptionStatus { get; set; }
        public string UserId { get; set; }
    }

    public class UpdateSubscriptionStatusCommandHandler : IRequestHandler<UpdateSubscriptionStatusCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        private readonly ISubscriberService _subscriberService;

        public UpdateSubscriptionStatusCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService, IConfiguration configuration, ISubscriberService subscriberService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
            _configuration = configuration;
            _subscriberService = subscriberService;
        }

        public async Task<Result> Handle(UpdateSubscriptionStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);
                SubscriberDto subscriber = _authService?.Subscriber;

                var entity = await _context.Subscriptions.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId
                && x.SubscriptionPlanId == request.SubscriptionPlanId && x.Id == request.Id);
                if (entity == null)
                {
                    return Result.Failure("Invalid Subscription match");
                }

                string message = "";
                switch (request.SubscriptionStatus)
                {
                    case SubscriptionStatus.Active:
                        entity.Status = Status.Active;
                        message += $"Subscription for subscriber {entity.SubscriberName} is now active!" + Environment.NewLine;
                        break;
                    case SubscriptionStatus.Cancelled:
                        entity.Status = Status.Deactivated;
                        message += $"Subscription for subscriber {entity.SubscriberName} is now cancelled!" + Environment.NewLine;
                        break;
                    case SubscriptionStatus.Expired:
                        entity.Status = Status.Deactivated;
                        message += $"Subscription for subscriber {entity.SubscriberName} is now expired!" + Environment.NewLine;
                        break;
                    case SubscriptionStatus.FreeTrial:
                        entity.Status = Status.Active;

                        entity.SubscriptionStatus = SubscriptionStatus.FreeTrial;
                        entity.StartDate = DateTime.Now;
                        int period = entity.SubscriptionPlan.FreeTrialDays == 0 ? _configuration["DefaultFreeTrialDays"].ToInt() : entity.SubscriptionPlan.FreeTrialDays;
                        entity.EndDate = DateTime.Now.AddDays(period);
                        entity.Status = Status.Active;
                        message = $"Subscription for subscriber {entity.SubscriberName} is now in free trial mode!" + Environment.NewLine;
                        break;
                    case SubscriptionStatus.ProcessingPayment:
                        entity.Status = subscriber.FreeTrialCompleted ? Status.Inactive : entity.Status = Status.Active;
                        message = $"Subscription for subscriber {entity.SubscriberName} is now been processed for payment!" + Environment.NewLine;
                        break;
                    default:
                        break;
                }

                if (entity.PaymentStatus == PaymentStatus.Success)
                {
                    entity.StartDate = DateTime.Now;
                    entity.SubscriptionStatus = SubscriptionStatus.Active;
                    entity.Status = Status.Active;
                    entity.EndDate = entity.StartDate.AddDays(entity.PaymentPeriod);
                }
                else if (entity.PaymentStatus == PaymentStatus.Initiated || entity.PaymentStatus == PaymentStatus.Processing)
                {
                    //If the subscriber's freetrial is still ongoing, leave the subscription to be active
                    //If the freetrial is completed, then update the subscription to be inactive or deactivate the subscription
                    if (subscriber.HasActivatedFreeTrial && !subscriber.FreeTrialCompleted)
                    {
                        entity.SubscriptionStatus = SubscriptionStatus.ProcessingPayment;
                        entity.Status = Status.Active;
                    }
                    if (subscriber.HasActivatedFreeTrial && subscriber.FreeTrialCompleted)
                    {
                        entity.SubscriptionStatus = SubscriptionStatus.ProcessingPayment;
                        entity.Status = Status.Inactive;
                    }
                }
                else if (entity.PaymentStatus == PaymentStatus.Failed || entity.PaymentStatus == PaymentStatus.Reversed || entity.PaymentStatus == PaymentStatus.Cancelled)
                {
                    //If the subscriber's freetrial is still ongoing, leave the subscription to be active
                    //If the freetrial is completed, then update the subscription to be inactive or deactivate the subscription
                    if (subscriber.HasActivatedFreeTrial && !subscriber.FreeTrialCompleted)
                    {
                        entity.SubscriptionStatus = SubscriptionStatus.Active;
                        entity.Status = Status.Active;
                    }
                    if (subscriber.HasActivatedFreeTrial && subscriber.FreeTrialCompleted)
                    {
                        entity.SubscriptionStatus = SubscriptionStatus.Expired;
                        entity.Status = Status.Deactivated;
                    }
                }

                entity.SubscriptionStatusDesc = entity.SubscriptionStatus.ToString();

                //If the subscriber's freetrial is still ongoing, leave the subscription to be active
                //If the freetrial is completed, then update the subscription to be inactive or deactivate the subscription
                if (subscriber.HasActivatedFreeTrial && !subscriber.FreeTrialCompleted)
                {
                    entity.SubscriptionStatus = SubscriptionStatus.Active;
                    entity.Status = Status.Active;
                }
                if (subscriber.HasActivatedFreeTrial && subscriber.FreeTrialCompleted)
                {
                    entity.SubscriptionStatus = SubscriptionStatus.Expired;
                    entity.Status = Status.Deactivated;
                }

                entity.SubscriptionStatus = request.SubscriptionStatus;
                entity.StatusDesc = entity.Status.ToString();
                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;

                //execute an update request to the subscriber profile to update the free trial activated field
                // TODO
                /*var freeTrialUpdateResult = await _subscriberService.ActivateSubscriberFreeTrialAsync(request.AccessToken, request.SubscriberId, request.UserId);
*/
                /*if (!freeTrialUpdateResult.Succeeded || freeTrialUpdateResult.Entity==null)
                {
                    return Result.Failure( freeTrialUpdateResult.Message );
                }*/
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<SubscriptionDto>(entity);
                return Result.Success(message, result);

            }
            catch (Exception ex)
            {
                return Result.Failure($"Subscription status update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }
    }
}
