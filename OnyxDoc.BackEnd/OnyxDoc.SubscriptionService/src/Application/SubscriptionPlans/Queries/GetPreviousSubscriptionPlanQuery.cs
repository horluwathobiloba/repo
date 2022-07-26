using AutoMapper;
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

namespace OnyxDoc.SubscriptionService.Application.SubscriptionPlans.Queries
{
    public class GetPreviousSubscriptionPlanQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string UserId { get; set; }
    }

    public class GetPreviousSubscriptionPlanQueryHandler : IRequestHandler<GetPreviousSubscriptionPlanQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetPreviousSubscriptionPlanQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetPreviousSubscriptionPlanQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var list = await _context.SubscriptionPlans.Include(a => a.SubscriptionPlanPricings).Include(a => a.SubscriptionPlanFeatures).
                           Where(a => a.SubscriberId == request.SubscriberId).LastOrDefaultAsync();
                var result = _mapper.Map<List<SubscriptionPlanDto>>(list);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving the last Subscription Plan. Error: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
