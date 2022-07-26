using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.SubscriptionService.Application.Common.Exceptions;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ReventInject;

namespace OnyxDoc.SubscriptionService.Application.SubscriptionPlans.Queries
{
    public class GetSubscriptionPlansDynamicQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string SearchText { get; set; }
        public string UserId { get; set; }
    }

    public class GetSubscriptionPlansDynamicQueryHandler : IRequestHandler<GetSubscriptionPlansDynamicQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetSubscriptionPlansDynamicQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetSubscriptionPlansDynamicQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId ,request.UserId);
                var list = await _context.SubscriptionPlans.Include(a => a.SubscriptionPlanPricings).Include(a => a.SubscriptionPlanFeatures).Where(a => a.SubscriberId == request.SubscriberId).ToListAsync();

                if (!string.IsNullOrEmpty(request.SearchText))
                {
                    list = list.Where(a => request.SearchText.IsIn(a.Name) 
                    || request.SearchText.IsIn(a.SubscriberName) 
                    || request.SearchText.IsIn(a.SubscriptionTypeDesc)).ToList();
                }

                if (list == null)
                {
                    throw new NotFoundException(nameof(SubscriptionPlan));
                }
                var result = _mapper.Map<List<SubscriptionPlanDto>>(list);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving SubscriptionPlans. Error: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }

}
