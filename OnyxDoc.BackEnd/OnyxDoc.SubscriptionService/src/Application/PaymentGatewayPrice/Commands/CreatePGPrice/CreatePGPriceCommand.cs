using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Application.PGPrices.Queries;
using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Domain.Enums;
using ReventInject;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.PGPrices.Commands
{
    public class CreatePGPriceCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public int SubscriptionPlanPricingId { get; set; }
        public int PaymentGatewayPriceId { get; set; }
        public string PaymentGatewayPriceCode { get; set; }
        public PaymentGateway PaymentGateway { get; set; }

        public string UserId { get; set; }
    }

    public class CreateSubscriptionCurrencyCommandHandler : IRequestHandler<CreatePGPriceCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreateSubscriptionCurrencyCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _authService = authService;
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(CreatePGPriceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var exists = await _context.PGPrices.AnyAsync(a => a.SubscriberId == request.SubscriberId
                && a.SubscriptionPlanPricingId == request.SubscriptionPlanPricingId && a.PaymentGatewayPriceCode == a.PaymentGatewayPriceCode && a.PaymentGateway == request.PaymentGateway);

                if (exists)
                {
                    return Result.Failure($"Price already exists for '{request.PaymentGateway}'.");
                }

                var entity = new PGPrice
                {
                    Name = request.PaymentGateway.ToString() + "-" + request.SubscriptionPlanPricingId,
                    SubscriberId = request.SubscriberId,
                    SubscriberName = _authService.Subscriber?.Name,
                    SubscriptionPlanPricingId = request.SubscriptionPlanPricingId,
                    PaymentGatewayPriceCode = request.PaymentGatewayPriceCode,
                    PaymentGateway = request.PaymentGateway,
                    PaymentGatewayPriceId = request.PaymentGatewayPriceId,

                    UserId = request.UserId,
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = request.UserId,
                    LastModifiedDate = DateTime.Now,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString()
                };

                await _context.PGPrices.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<PGPriceDto>(entity);
                return Result.Success("Payment gateway price created successfully!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Payment gateway price creation failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
