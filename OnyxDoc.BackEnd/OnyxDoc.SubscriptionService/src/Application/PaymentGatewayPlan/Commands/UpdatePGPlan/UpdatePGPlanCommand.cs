using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Application.PGPlans.Queries;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.PGPlans.Commands
{
    public class UpdatePGPlanCommand : AuthToken, IRequest<Result>
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public int SubscriptionId { get; set; }
        public int PaymentGatewayPlanId { get; set; }
        public string PaymentGatewayPlanCode { get; set; }

        public PaymentGateway PaymentGateway { get; set; }

        public string UserId { get; set; }
    }

    public class UpdateSubscriptionCurrencyCommandHandler : IRequestHandler<UpdatePGPlanCommand, Result>
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
        public async Task<Result> Handle(UpdatePGPlanCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var entity = await _context.PGPlans.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId && x.SubscriptionId == request.SubscriptionId
                 && (x.Id == request.Id || (x.PaymentGatewayPlanCode == request.PaymentGatewayPlanCode && x.PaymentGateway == request.PaymentGateway)));

                if (entity == null)
                {
                    return Result.Failure($"Invalid plan specified.");
                }

                entity.SubscriptionId = request.SubscriptionId;
                entity.PaymentGatewayPlanId = request.PaymentGatewayPlanId;
                entity.PaymentGatewayPlanCode = request.PaymentGatewayPlanCode;
                entity.PaymentGateway = request.PaymentGateway;
                entity.PaymentGatewayDesc = request.PaymentGateway.ToString();
                entity.UserId = request.UserId;
                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;

                _context.PGPlans.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<PGPlanDto>(entity);
                return Result.Success("Payment gateway plan update was successful!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Payment gateway plan update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }

    }


}
