using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.SubscriptionService.Application.Common.Interfaces;
using RubyReloaded.SubscriptionService.Application.Common.Models;
using RubyReloaded.SubscriptionService.Application.PaymentChannels.Queries;
using RubyReloaded.SubscriptionService.Domain.Entities;
using RubyReloaded.SubscriptionService.Domain.Enums;
using ReventInject;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.SubscriptionService.Application.PaymentChannels.Commands
{
    public class CreatePaymentChannelCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string Name { get; set; }
        public TransactionRateType TransactionRateType { get; set; }
        public CurrencyCode CurrencyCode { get; set; }
        public decimal TransactionFee { get; set; }
        public string UserId { get; set; }
    }

    public class CreatePaymentChannelCommandHandler : IRequestHandler<CreatePaymentChannelCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreatePaymentChannelCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _authService = authService;
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(CreatePaymentChannelCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var currency = await _context.Currencies.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId && x.CurrencyCode == request.CurrencyCode);

                if (currency == null || currency.Id <= 0)
                {
                    return Result.Failure("Currency specified is not configured.");
                }

                var exists = await _context.PaymentChannels.AnyAsync(x => x.SubscriberId == request.SubscriberId && x.Name == request.Name);
                if (exists)
                {
                    return Result.Failure($"Currency code '{request.CurrencyCode}' or payment channel name '{request.Name}' already exists.");
                }

                var entity = new PaymentChannel
                {
                    Name = request.Name,
                    CurrencyId = currency.Id,
                    SubscriberId = request.SubscriberId,
                    SubscriberName = _authService.Subscriber?.Name,
                    CurrencyCode = request.CurrencyCode,
                    CurrencyCodeDesc = request.CurrencyCode.ToString(),
                    TransactionRateType = request.TransactionRateType,
                    TransactionRateTypeDesc = request.TransactionRateType.ToString(),
                    TransactionFee = request.TransactionFee,
                    UserId = request.UserId,
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = request.UserId,
                    LastModifiedDate = DateTime.Now,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString()
                };

                await _context.PaymentChannels.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<PaymentChannelDto>(entity);
                return Result.Success("Payment channel created successfully!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Payment channel creation failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
