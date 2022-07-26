using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.SubscriptionService.Application.Common.Interfaces;
using RubyReloaded.SubscriptionService.Application.Common.Models;
using RubyReloaded.SubscriptionService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.SubscriptionService.Application.Subscriptions.Queries
{
    public class GetSubscriptionsByPlanQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int SubscriptionPlanId { get; set; }
        public string UserId { get; set; }
    }
    public class GetSubscriptionsByPlanQueryHandler : IRequestHandler<GetSubscriptionsByPlanQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;
        public GetSubscriptionsByPlanQueryHandler(IApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        public async Task<Result> Handle(GetSubscriptionsByPlanQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId ,request.UserId);

                var entity = await _context.Subscriptions.Where(x => x.SubscriberId == request.SubscriberId && x.SubscriptionPlanId == request.SubscriberId).ToListAsync();

                if (entity == null || entity.Count == 0)
                {
                    return Result.Failure("No record found");
                }
                return Result.Success($"{entity.Count}(s) found.", entity);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Get subscriptions by subscription plan failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
