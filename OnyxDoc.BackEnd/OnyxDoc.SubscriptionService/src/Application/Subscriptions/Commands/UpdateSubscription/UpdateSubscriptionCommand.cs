using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Application.Subscriptions.Queries;
using OnyxDoc.SubscriptionService.Application.Utilities.Queries;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.Subscriptions.Commands
{
    public class UpdateSubscriptionCommand : AuthToken, IRequest<Result>
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public int SystemSettingId { get; set; }
        public int SubscriptionPlanPricingId { get; set; }
        public string SubscriberName { get; set; }
        public int SubscriptionPlanId { get; set; }
        public int? InitialSubscriptionPlanId { get; set; }
        public int? RenewedSubscriptionPlanId { get; set; }
        public int CurrencyId { get; set; }
        public int PaymentChannelId { get; set; }
        public string CurrencyCode { get; set; }
        public string TransactionReference { get; set; }
        public string PaymentChannelReference { get; set; }
        public string PaymentChannelResponse { get; set; }
        public string PaymentChannelStatus { get; set; }
        public decimal Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string PaymentStatusDesc { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
        public string SubscriptionTypeDesc { get; set; }
        public string UserId { get; set; }
        public bool IsHighlighted { get; set; }
    }

    public class UpdateSubscriptionCommandHandler : IRequestHandler<UpdateSubscriptionCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public UpdateSubscriptionCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(UpdateSubscriptionCommand request, CancellationToken cancellationToken)
        {
            try
            {


                var currency = new GetCurrencyBySystemSettingsQuery
                {
                    SubscriberId = request.SubscriberId,
                    UserId = request.UserId,
                    SystemSettingsId = request.SystemSettingId
                };


                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId ,request.UserId);

                var UpdatedEntityExists = await _context.Subscriptions
                       .AnyAsync(x => x.SubscriberId == request.SubscriberId && x.SubscriptionPlanId == request.SubscriptionPlanId && x.CurrencyId == request.CurrencyId);

                if (UpdatedEntityExists)
                {
                    return Result.Failure($"The payment named '{request.CurrencyCode}' is already configured for this subscription.");
                }

                var entity = await _context.Subscriptions.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId
                && x.SubscriptionPlanId == request.SubscriptionPlanId && x.Id == request.Id);

                if (entity == null)
                {
                    return Result.Failure($"Invalid subscription specified.");
                }

                entity.CurrencyId = request.CurrencyId;
                entity.CurrencyCode = request.CurrencyCode;
                entity.Amount = request.Amount;
                 
                entity.PaymentChannelId = request.PaymentChannelId;
                entity.SubscriptionPlanPricingId = request.SubscriptionPlanPricingId;
                entity.PaymentChannelReference = request.PaymentChannelReference;
                entity.PaymentChannelResponse = request.PaymentChannelResponse;
                entity.PaymentChannelStatus = request.PaymentChannelStatus;
                entity.PaymentStatus = request.PaymentStatus;
                entity.RenewedSubscriptionPlanId = request.RenewedSubscriptionPlanId;
                entity.StartDate = request.StartDate;
                entity.EndDate = request.EndDate;
                entity.SubscriptionType = request.SubscriptionType;
                entity.SubscriptionTypeDesc = request.SubscriptionType.ToString();
                entity.TransactionReference = request.TransactionReference; 

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
