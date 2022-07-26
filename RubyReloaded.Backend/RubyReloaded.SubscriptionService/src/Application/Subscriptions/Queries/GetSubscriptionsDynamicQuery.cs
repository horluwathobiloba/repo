using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.SubscriptionService.Application.Common.Exceptions;
using RubyReloaded.SubscriptionService.Application.Common.Interfaces;
using RubyReloaded.SubscriptionService.Application.Common.Models;
using RubyReloaded.SubscriptionService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ReventInject;

namespace RubyReloaded.SubscriptionService.Application.Subscriptions.Queries
{
    public class GetSubscriptionsDynamicQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string SearchText { get; set; }
        public string UserId { get; set; }
    }

    public class GeSubscriptionsDynamicQueryHandler : IRequestHandler<GetSubscriptionsDynamicQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GeSubscriptionsDynamicQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetSubscriptionsDynamicQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId,  request.UserId);
                var list = await _context.Subscriptions
                    .Include(a => a.SubscriptionPlan)
                    .Include(a => a.Currency)
                    .Where(a => a.SubscriberId == request.SubscriberId).ToListAsync();

                if (!string.IsNullOrEmpty(request.SearchText))
                { 
                    list = list.Where(a => request.SearchText.IsIn(a.Name)
                    || request.SearchText.IsIn(a.TransactionReference)
                    || request.SearchText.IsIn(a.PaymentChannelReference)
                    || request.SearchText.IsIn(a.PaymentStatusDesc)
                    || request.SearchText.IsIn(a.PaymentChannelStatus)
                    || request.SearchText.IsIn(a.SubscriptionTypeDesc)).ToList();
                }

                if (list == null)
                {
                    throw new NotFoundException(nameof(Subscription));
                }
                var result = _mapper.Map<List<SubscriptionDto>>(list);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving subscription. Error: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }

}
