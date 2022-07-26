using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Application.PGPrices.Queries;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.PGPrices.Commands
{
    public class UpdatePGPriceCommand : AuthToken, IRequest<Result>
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public int SubscriptionPlanPricingId { get; set; }
        public int PaymentGatewayPriceId { get; set; }
        public string PaymentGatewayPriceCode { get; set; }

        public PaymentGateway PaymentGateway { get; set; }

        public string UserId { get; set; }
    }

    public class UpdateSubscriptionCurrencyCommandHandler : IRequestHandler<UpdatePGPriceCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public UpdateSubscriptionCurrencyCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(UpdatePGPriceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);              

                var entity = await _context.PGPrices.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId && x.SubscriptionPlanPricingId == request.SubscriptionPlanPricingId
                && (x.Id == request.Id || (x.PaymentGatewayPriceCode == request.PaymentGatewayPriceCode && x.PaymentGateway == request.PaymentGateway)));

                if (entity == null)
                {
                    return Result.Failure($"Invalid payment gateway price specified.");
                }

                entity.SubscriptionPlanPricingId = request.SubscriptionPlanPricingId;
                entity.PaymentGatewayPriceId = request.PaymentGatewayPriceId;
                entity.PaymentGateway = request.PaymentGateway;

                entity.PaymentGatewayDesc = request.PaymentGateway.ToString();
                entity.UserId = request.UserId;
                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;

                _context.PGPrices.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<PGPriceDto>(entity);
                return Result.Success("Payment gateway price update was successful!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Payment gateway price update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }

    }


}
