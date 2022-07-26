using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Application.PGProducts.Queries;
using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Domain.Enums;
using ReventInject;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.PGProducts.Commands
{
    public class CreatePGProductCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public int SubscriptionPlanId { get; set; }
        public int PaymentGatewayProductId { get; set; }
        public string PaymentGatewayProductCode { get; set; }
        public PaymentGateway PaymentGateway { get; set; }

        public string UserId { get; set; }
    }

    public class CreatePGProductCommandHandler : IRequestHandler<CreatePGProductCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreatePGProductCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _authService = authService;
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(CreatePGProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var exists = await _context.PGProducts.AnyAsync(a => a.SubscriberId == request.SubscriberId
                && a.SubscriptionPlanId == request.SubscriptionPlanId && a.PaymentGatewayProductCode == a.PaymentGatewayProductCode && a.PaymentGateway == request.PaymentGateway);
               
                if (exists)
                {
                    return Result.Failure($"Product already exists for '{request.PaymentGateway}'.");
                }

                var entity = new PGProduct
                {
                    Name = request.PaymentGateway.ToString() + "-" + request.SubscriptionPlanId,
                    SubscriberId = request.SubscriberId,
                    SubscriberName = _authService.Subscriber?.Name,
                    SubscriptionPlanId = request.SubscriptionPlanId,
                    PaymentGateway = request.PaymentGateway,
                    PaymentGatewayProductId = request.PaymentGatewayProductId,
                    PaymentGatewayProductCode = request.PaymentGatewayProductCode,

                    UserId = request.UserId,
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = request.UserId,
                    LastModifiedDate = DateTime.Now,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString()
                };

                await _context.PGProducts.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<PGProductDto>(entity);
                return Result.Success("Payment gateway product created successfully!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Payment gateway product creation failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
