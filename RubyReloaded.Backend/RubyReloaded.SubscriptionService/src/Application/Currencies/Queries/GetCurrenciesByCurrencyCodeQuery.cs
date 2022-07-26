using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.SubscriptionService.Application.Common.Interfaces;
using RubyReloaded.SubscriptionService.Application.Common.Models; 
using System; 
using System.Linq; 
using System.Threading;
using System.Threading.Tasks;
using ReventInject;
using RubyReloaded.SubscriptionService.Domain.Enums;

namespace RubyReloaded.SubscriptionService.Application.Currencies.Queries
{
    public class GetCurrenciesByCurrencyCodeQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; } 
        public string CurrencyCode { get; set; }
        public string UserId { get; set; }
    }
    public class GetCurrenciesByCurrencyCodeQueryHandler : IRequestHandler<GetCurrenciesByCurrencyCodeQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;
        public GetCurrenciesByCurrencyCodeQueryHandler(IApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        public async Task<Result> Handle(GetCurrenciesByCurrencyCodeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var entity = await _context.Currencies
                    .Where(x => x.SubscriberId == request.SubscriberId  
                    && x.CurrencyCode == request.CurrencyCode.ParseEnum<CurrencyCode>()).ToListAsync();

                if (entity == null || entity.Count == 0)
                {
                    return Result.Failure("No Currencies available");
                }
                return Result.Success($"{entity.Count}(s) found.", entity);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Get Currencies failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
