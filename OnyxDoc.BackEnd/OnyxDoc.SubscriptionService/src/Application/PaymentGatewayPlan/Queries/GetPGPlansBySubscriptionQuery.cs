using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.PGPlans.Queries
{
    public class GetPGPlansBySubscriptionPlanQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int SubscriptionId { get; set; }
        public string UserId { get; set; }
    }
    public class GetPGPlansBySubscriptionPlanQueryHandler : IRequestHandler<GetPGPlansBySubscriptionPlanQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;
        public GetPGPlansBySubscriptionPlanQueryHandler(IApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        public async Task<Result> Handle(GetPGPlansBySubscriptionPlanQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId ,request.UserId);

                var entity = await _context.PGPlans.Where(x => x.SubscriberId == request.SubscriberId && x.SubscriptionId == request.SubscriberId).ToListAsync();

                if (entity == null || entity.Count == 0)
                {
                    return Result.Failure("No record found");
                }
                return Result.Success($"{entity.Count}(s) found.", entity);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Get payment gateway plans failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
