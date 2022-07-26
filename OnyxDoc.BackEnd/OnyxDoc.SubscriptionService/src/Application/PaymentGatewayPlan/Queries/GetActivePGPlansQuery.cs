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

namespace OnyxDoc.SubscriptionService.Application.PGPlans.Queries
{
    public class GetActivePGPlansQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int SubscriptionId { get; set; }
        public string UserId { get; set; }
    }

    public class GetActivePGPlansQueryHandler : IRequestHandler<GetActivePGPlansQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetActivePGPlansQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetActivePGPlansQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId ,request.UserId);

                var list = await _context.PGPlans.Where(a => a.SubscriberId == request.SubscriberId && a.SubscriptionId == request.SubscriptionId && a.Status == Status.Active).ToListAsync();
                var result = _mapper.Map<List<PGPlanDto>>(list);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving payment gateway plans. Error: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
