using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Application.PGSubscriptions.Queries;
using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Domain.Enums;
using ReventInject;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.PGSubscriptions.Commands
{
    public class CreatePGSubscriptionCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public int SubscriptionId { get; set; }
        public int PaymentGatewaySubscriptionId { get; set; }
        public string PaymentGatewaySubscriptionCode { get; set; }
        public PaymentGateway PaymentGateway { get; set; }

        public string UserId { get; set; }
    }

    public class CreateSubscriptionCurrencyCommandHandler : IRequestHandler<CreatePGSubscriptionCommand, Result>
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

        public async Task<Result> Handle(CreatePGSubscriptionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var exists = await _context.PGSubscriptions.AnyAsync(a => a.SubscriberId == request.SubscriberId
                && a.SubscriptionId == request.SubscriptionId && a.PaymentGatewaySubscriptionCode == a.PaymentGatewaySubscriptionCode && a.PaymentGateway == request.PaymentGateway);

                if (exists)
                {
                    return Result.Failure($"Subscription already exists for '{request.PaymentGateway}'.");
                }

                PGSubscription pgSubscription = await _context.PGSubscriptions.FirstOrDefaultAsync(a => a.SubscriberId == request.SubscriberId && a.SubscriptionId == request.SubscriptionId && a.PaymentGateway == request.PaymentGateway);

                var entity = new PGSubscription
                {
                    Name = request.PaymentGateway.ToString() + "-" + request.SubscriptionId,
                    SubscriberId = request.SubscriberId,
                    SubscriberName = _authService.Subscriber?.Name,
                    SubscriptionId = request.SubscriptionId,
                    PaymentGateway = request.PaymentGateway,
                    PaymentGatewaySubscriptionCode = request.PaymentGatewaySubscriptionCode,
                    PaymentGatewaySubscriptionId = request.PaymentGatewaySubscriptionId,

                    UserId = request.UserId,
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = request.UserId,
                    LastModifiedDate = DateTime.Now,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString()
                };

                await _context.PGSubscriptions.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<PGSubscriptionDto>(entity);
                return Result.Success("Payment gateway subscription created successfully!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Payment gateway subscription creation failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
