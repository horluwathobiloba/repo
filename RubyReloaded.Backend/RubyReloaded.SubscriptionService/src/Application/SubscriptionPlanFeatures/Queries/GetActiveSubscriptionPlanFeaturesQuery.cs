using AutoMapper;
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

namespace RubyReloaded.SubscriptionService.Application.SubscriptionPlanFeatures.Queries
{
    public class GetActiveSubscriptionPlanFeaturesQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int SubscriptionPlanId { get; set; }
        public string UserId { get; set; }
    }

    public class GetActiveSubscriptionPlanFeaturesQueryHandler : IRequestHandler<GetActiveSubscriptionPlanFeaturesQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetActiveSubscriptionPlanFeaturesQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetActiveSubscriptionPlanFeaturesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var list = await _context.SubscriptionPlanFeatures.Where(a => a.SubscriberId == request.SubscriberId && a.SubscriptionPlanId == request.SubscriptionPlanId && a.Status == Status.Active).ToListAsync();
                var result = _mapper.Map<List<SubscriptionPlanFeatureDto>>(list);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving subscription plan features. Error: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
