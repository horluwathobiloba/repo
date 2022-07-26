using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Domain.Enums;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.PaymentChannels.Queries
{
    public class GetPaymentChannelsByCurrencyQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; } 
        public string CurrencyCode { get; set; }
        public string UserId { get; set; }
    }

    public class GetPaymentChannelsByCurrencyQueryHandler : IRequestHandler<GetPaymentChannelsByCurrencyQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;
        public GetPaymentChannelsByCurrencyQueryHandler(IApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        public async Task<Result> Handle(GetPaymentChannelsByCurrencyQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var entity = await _context.PaymentChannels
                    .Where(x => x.SubscriberId == request.SubscriberId
                    && x.CurrencyCode == request.CurrencyCode.ParseEnum<CurrencyCode>()).ToListAsync();

                if (entity == null || entity.Count == 0)
                {
                    return Result.Failure("No PaymentChannels available");
                }
                return Result.Success($"{entity.Count}(s) found.", entity);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Get PaymentChannels failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
