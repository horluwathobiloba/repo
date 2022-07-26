using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Application.PGCustomers.Queries;
using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Domain.Enums;
using ReventInject;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.PGCustomers.Commands
{
    public class CreatePGCustomerCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public int SubscriptionId { get; set; }
        public string PaymentGatewayCustomerCode { get; set; }
        public int PaymentGatewayCustomerId { get; set; }
        public PaymentGateway PaymentGateway { get; set; }

        public string UserId { get; set; }
    }

    public class CreateSubscriptionCurrencyCommandHandler : IRequestHandler<CreatePGCustomerCommand, Result>
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

        public async Task<Result> Handle(CreatePGCustomerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var exists = await _context.PGCustomers.AnyAsync(a => a.SubscriberId == request.SubscriberId && a.PaymentGatewayCustomerCode == a.PaymentGatewayCustomerCode && a.PaymentGateway == request.PaymentGateway);

                if (exists)
                {
                    return Result.Failure($"Customer already exists for '{request.PaymentGateway}'.");
                }

                PGCustomer subscriptionPlan = await _context.PGCustomers.FirstOrDefaultAsync(a => a.SubscriberId == request.SubscriberId && a.PaymentGatewayCustomerCode == request.PaymentGatewayCustomerCode && a.PaymentGateway == request.PaymentGateway);

                var entity = new PGCustomer
                {
                    Name = request.PaymentGateway.ToString() + "-" + request.SubscriptionId,
                    SubscriberId = request.SubscriberId,
                    SubscriberName = _authService.Subscriber?.Name,
                    PaymentGateway = request.PaymentGateway,
                    PaymentGatewayCustomerCode = request.PaymentGatewayCustomerCode,
                    PaymentGatewayCustomerId = request.PaymentGatewayCustomerId,

                    UserId = request.UserId,
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = request.UserId,
                    LastModifiedDate = DateTime.Now,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString()
                };

                await _context.PGCustomers.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<PGCustomerDto>(entity);
                return Result.Success("Payment gateway customer created successfully!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Payment gateway customer creation failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
