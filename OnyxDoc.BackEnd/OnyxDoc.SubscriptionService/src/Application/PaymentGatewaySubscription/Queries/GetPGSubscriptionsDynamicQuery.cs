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

namespace OnyxDoc.SubscriptionService.Application.PGSubscriptions.Queries
{
    public class GetPGSubscriptionsDynamicQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string SearchText { get; set; }
        public string UserId { get; set; }
    }

    public class GePGSubscriptionsDynamicQueryHandler : IRequestHandler<GetPGSubscriptionsDynamicQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GePGSubscriptionsDynamicQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetPGSubscriptionsDynamicQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);
                var list = await _context.PGSubscriptions.Where(a => a.SubscriberId == request.SubscriberId)
                    .Include(b => b.Subscription).ToListAsync();

                if (!string.IsNullOrEmpty(request.SearchText))
                {
                    list = list.Where(a => request.SearchText.IsIn(a.PaymentGateway.ToString())).ToList();
                }

                if (list == null)
                {
                    throw new NotFoundException(nameof(PGSubscription));
                }
                var result = _mapper.Map<List<PGSubscriptionDto>>(list);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving payment gateway subscriptions. Error: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }

}
