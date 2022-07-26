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

namespace OnyxDoc.SubscriptionService.Application.Subscriptions.Queries
{
    public class GetActiveSubscriptionBySubscriberIdQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string UserId { get; set; }
    }

    public class GetActiveSubscriptionBySubscriberIdQueryHandler : IRequestHandler<GetActiveSubscriptionBySubscriberIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetActiveSubscriptionBySubscriberIdQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetActiveSubscriptionBySubscriberIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);
                var subscription = await _context.Subscriptions.Where(a => a.SubscriberId == request.SubscriberId && a.SubscriptionStatus == SubscriptionStatus.Active)
                    .OrderBy(a=>a.CreatedDate).FirstOrDefaultAsync();
                var result = _mapper.Map<SubscriptionDto>(subscription);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving subscription. Error: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
