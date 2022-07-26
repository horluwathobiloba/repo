using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.SubscriptionService.Application.Common.Interfaces;
using RubyReloaded.SubscriptionService.Application.Common.Models;
using RubyReloaded.SubscriptionService.Application.Subscriptions.Queries;
using RubyReloaded.SubscriptionService.Domain.Entities;
using RubyReloaded.SubscriptionService.Domain.Enums;
using ReventInject;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.SubscriptionService.Application.Subscriptions.Commands
{
    public class RenewSubscriptionCommand : AuthToken, IRequest<Result>
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
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
    }

    public class RenewSubscriptionCommandHandler : IRequestHandler<RenewSubscriptionCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public RenewSubscriptionCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _authService = authService;
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(RenewSubscriptionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId ,request.UserId);

                var exists = await _context.Subscriptions.AnyAsync(a => a.SubscriberId == request.SubscriberId
                && a.SubscriptionPlanId == request.SubscriptionPlanId && (a.TransactionReference == request.TransactionReference || a.PaymentChannelReference == request.PaymentChannelReference));

                if (exists)
                {
                    return Result.Failure($"Subscription with reference number:'{request.TransactionReference}' already exists.");
                }

                var entity = new Subscription
                {
                    Name = request.SubscriberId + "_" + request.SubscriptionPlanId,
                    SubscriberId = request.SubscriberId,
                    SubscriberName = _authService.Subscriber?.Name,
                    SubscriptionPlanId = request.SubscriptionPlanId,
                    CurrencyCode = request.CurrencyCode,
                    CurrencyId = request.CurrencyId,
                    Amount = request.Amount,
                    InitialSubscriptionPlanId = request.InitialSubscriptionPlanId,
                    PaymentChannelId = request.PaymentChannelId,
                    PaymentChannelReference = request.PaymentChannelReference,
                    PaymentChannelResponse = request.PaymentChannelResponse,
                    PaymentChannelStatus = request.PaymentChannelStatus,
                    PaymentStatus = request.PaymentStatus,
                    PaymentStatusDesc = request.PaymentStatus.ToString(),
                    RenewedSubscriptionPlanId = request.RenewedSubscriptionPlanId,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    SubscriptionType = request.SubscriptionType,
                    SubscriptionTypeDesc = request.SubscriptionType.ToString(),
                    TransactionReference = request.TransactionReference,                     

                    UserId = request.UserId,
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = request.UserId,
                    LastModifiedDate = DateTime.Now,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString()
                };

                await _context.Subscriptions.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<SubscriptionDto>(entity);
                return Result.Success("Subscription created successfully!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Subscription creation failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
