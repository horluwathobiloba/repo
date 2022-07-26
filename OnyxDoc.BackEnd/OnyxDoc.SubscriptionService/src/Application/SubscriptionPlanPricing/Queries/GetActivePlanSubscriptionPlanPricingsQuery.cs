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

namespace OnyxDoc.SubscriptionService.Application.SubscriptionPlanPricings.Queries
{
    public class GetActiveSubscriptionPlanPricingsQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int SubscriptionPlanId { get; set; }
        public string UserId { get; set; }
    }

    public class GetActiveSubscriptionPlanPricingsQueryHandler : IRequestHandler<GetActiveSubscriptionPlanPricingsQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetActiveSubscriptionPlanPricingsQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetActiveSubscriptionPlanPricingsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId ,request.UserId);

                var list = await _context.SubscriptionPlanPricings.Where(a => a.SubscriberId == request.SubscriberId && a.SubscriptionPlanId == request.SubscriptionPlanId && a.Status == Status.Active).ToListAsync();
                var result = _mapper.Map<List<SubscriptionPlanPricingDto>>(list);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving subscription plan pricings. Error: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
