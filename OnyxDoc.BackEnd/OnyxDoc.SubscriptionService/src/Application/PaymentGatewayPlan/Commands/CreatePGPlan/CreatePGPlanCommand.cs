using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Application.PGPlans.Queries;
using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Domain.Enums;
using ReventInject;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.PGPlans.Commands
{
    public class CreatePGPlanCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public int SubscriptionId { get; set; }
        public int PaymentGatewayPlanId { get; set; }
        public string PaymentGatewayPlanCode { get; set; }
        public PaymentGateway PaymentGateway { get; set; }

        public string UserId { get; set; }
    }

    public class CreatePGPlanCommandHandler : IRequestHandler<CreatePGPlanCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreatePGPlanCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _authService = authService;
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(CreatePGPlanCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var exists = await _context.PGPlans.AnyAsync(a => a.SubscriberId == request.SubscriberId
                && a.SubscriptionId == request.SubscriptionId && a.PaymentGatewayPlanCode == a.PaymentGatewayPlanCode && a.PaymentGateway == request.PaymentGateway);

                if (exists)
                {
                    return Result.Failure($"Plan already exists for '{request.PaymentGateway}'.");
                }

                SubscriptionPlan subscriptionPlan = await _context.SubscriptionPlans.FirstOrDefaultAsync(a => a.Id == request.SubscriptionId);

                var entity = new PGPlan
                {
                    Name = request.PaymentGateway.ToString() + "-" + request.SubscriptionId,
                    SubscriberId = request.SubscriberId,
                    SubscriberName = _authService.Subscriber?.Name,
                    SubscriptionId = request.SubscriptionId,
                    PaymentGateway = request.PaymentGateway,
                    PaymentGatewayPlanId = request.PaymentGatewayPlanId,
                    PaymentGatewayPlanCode = request.PaymentGatewayPlanCode,

                    UserId = request.UserId,
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = request.UserId,
                    LastModifiedDate = DateTime.Now,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString()
                };

                await _context.PGPlans.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<PGPlanDto>(entity);
                return Result.Success("Payment gateway plan created successfully!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Payment gateway plan creation failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
