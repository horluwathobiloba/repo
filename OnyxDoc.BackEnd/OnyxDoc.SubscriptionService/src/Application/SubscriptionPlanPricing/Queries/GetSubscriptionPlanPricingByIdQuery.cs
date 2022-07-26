using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.SubscriptionService.Application.Common.Exceptions;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.SubscriptionPlanPricings.Queries
{
    public class GetSubscriptionPlanPricingByIdQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int SubscriptionPlanId { get; set; }
        public int Id { get; set; }
        public string UserId { get; set; }
    }

    public class GetSubscriptionCurrencyByIdQueryHandler : IRequestHandler<GetSubscriptionPlanPricingByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetSubscriptionCurrencyByIdQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetSubscriptionPlanPricingByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId ,request.UserId);

                var SubscriptionCurrency = await _context.SubscriptionPlanPricings.FirstOrDefaultAsync(a => a.SubscriberId == request.SubscriberId 
                && a.SubscriptionPlanId ==  request.SubscriptionPlanId  && a.Id == request.Id);
                if (SubscriptionCurrency == null)
                {
                    return Result.Failure("No record found!");
                }

                var result = _mapper.Map<SubscriptionPlanPricingDto>(SubscriptionCurrency);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving subscription plan pricing. Error: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
