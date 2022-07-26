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

namespace RubyReloaded.SubscriptionService.Application.Currencies.Queries
{
    public class GetActiveCurrencyNamesQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string UserId { get; set; }
    }

    public class GetActiveCurrencyNamesQueryHandler : IRequestHandler<GetActiveCurrencyNamesQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetActiveCurrencyNamesQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetActiveCurrencyNamesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);
                var result = await _context.Currencies.Where(a => a.SubscriberId == request.SubscriberId
                && a.Status == Status.Active)
                    .Select(b => b.Name).ToListAsync();

                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving Currencies. Error: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
