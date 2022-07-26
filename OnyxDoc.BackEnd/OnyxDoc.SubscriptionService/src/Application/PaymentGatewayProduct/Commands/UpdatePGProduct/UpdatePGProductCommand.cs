using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Application.PGProducts.Queries;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.PGProducts.Commands
{
    public class UpdatePGProductCommand : AuthToken, IRequest<Result>
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public int SubscriptionPlanId { get; set; }
        public int PaymentGatewayProductId { get; set; }
        public string PaymentGatewayProductCode { get; set; }

        public PaymentGateway PaymentGateway { get; set; }

        public string UserId { get; set; }
    }

    public class UpdateSubscriptionCurrencyCommandHandler : IRequestHandler<UpdatePGProductCommand, Result>
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
        public async Task<Result> Handle(UpdatePGProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var entity = await _context.PGProducts.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId && x.SubscriptionPlanId == request.SubscriptionPlanId
                && (x.Id == request.Id || (x.PaymentGatewayProductCode == request.PaymentGatewayProductCode && x.PaymentGateway == request.PaymentGateway)));

                if (entity == null)
                {
                    return Result.Failure($"Invalid payment gateway product specified.");
                }

                entity.PaymentGatewayProductId = request.PaymentGatewayProductId;
                entity.PaymentGatewayProductCode = request.PaymentGatewayProductCode;
                entity.PaymentGateway = request.PaymentGateway;

                entity.PaymentGatewayDesc = request.PaymentGateway.ToString();
                entity.UserId = request.UserId;
                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;

                _context.PGProducts.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<PGProductDto>(entity);
                return Result.Success("Payment gateway product update was successful!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Payment gateway product update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }

    }


}
