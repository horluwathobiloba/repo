using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Application.Subscriptions.Queries;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.Subscriptions.Commands
{
    public class UpdateSubscriptionPaymentCommand : AuthToken, IRequest<Result>
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public int SubscriptionPlanId { get; set; }
        public int SubscriptionPlanPricingId { get; set; }

        public int PaymentChannelId { get; set; }
        public string TransactionReference { get; set; }
        public string PaymentChannelReference { get; set; }
        public string PaymentChannelResponse { get; set; }
        public string PaymentChannelStatus { get; set; }
        public decimal Amount { get; set; }
        public decimal TransactionFee { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public PaymentStatus PaymentStatus { get; set; } 
        public SubscriptionType SubscriptionType { get; set; } 
        public SubscriptionFrequency SubscriptionFrequency { get; set; } 
        public int PaymentPeriod { get; set; } 

        public string UserId { get; set; }
    }

    public class UpdateSubscriptionPaymentCommandHandler : IRequestHandler<UpdateSubscriptionPaymentCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public UpdateSubscriptionPaymentCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(UpdateSubscriptionPaymentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);
                var subscriber = _authService?.Subscriber;

                var entity = await _context.Subscriptions.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId
                && x.SubscriptionPlanId == request.SubscriptionPlanId && x.Id == request.Id);

                if (entity == null)
                {
                    return Result.Failure($"Invalid subscription specified.");
                }

                entity.Amount = request.Amount;
                entity.SubscriptionPlanPricingId = request.SubscriptionPlanPricingId;
                entity.PaymentPeriod = request.PaymentPeriod;
                entity.SubscriptionFrequency = request.SubscriptionFrequency;
                entity.SubscriptionFrequencyDesc = request.SubscriptionFrequency.ToString();
                entity.PaymentChannelId = request.PaymentChannelId;
                entity.TransactionReference = request.TransactionReference;
                entity.PaymentChannelReference = request.PaymentChannelReference;
                entity.PaymentChannelResponse = request.PaymentChannelResponse;
                entity.PaymentChannelStatus = request.PaymentChannelStatus;

                entity.PaymentStatus = request.PaymentStatus;
                entity.PaymentStatusDesc = request.PaymentStatus.ToString();
                entity.SubscriptionType = request.SubscriptionType;
                entity.SubscriptionTypeDesc = request.SubscriptionType.ToString();

                if (request.PaymentStatus == PaymentStatus.Success)
                {
                    entity.StartDate = DateTime.Now;
                    entity.SubscriptionStatus = SubscriptionStatus.Active; 
                    entity.Status = Status.Active;
                    entity.EndDate = request.StartDate.AddDays(entity.PaymentPeriod);
                }
                else if (request.PaymentStatus == PaymentStatus.Initiated || request.PaymentStatus == PaymentStatus.Processing)
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
                else if(request.PaymentStatus == PaymentStatus.Failed || request.PaymentStatus == PaymentStatus.Reversed || request.PaymentStatus == PaymentStatus.Cancelled)
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
                entity.StatusDesc = entity.Status.ToString();
                entity.UserId = request.UserId;
                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;

                _context.Subscriptions.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<SubscriptionDto>(entity);
                return Result.Success("Subscription update was successful!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Subscription update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }

    }


}
