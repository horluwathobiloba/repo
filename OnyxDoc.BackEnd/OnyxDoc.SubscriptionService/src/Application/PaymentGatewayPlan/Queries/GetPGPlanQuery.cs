﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.SubscriptionService.Application.Common.Exceptions;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.PGPlans.Queries
{
    public class GetPGPlanQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int SubscriptionId { get; set; }
        public PaymentGateway PaymentGateway { get; set; }
        public string UserId { get; set; }
    }

    public class GetPGPlanQueryHandler : IRequestHandler<GetPGPlanQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetPGPlanQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetPGPlanQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId ,request.UserId);

                var PGPlan = await _context.PGPlans.FirstOrDefaultAsync(a => a.SubscriberId == request.SubscriberId 
                && a.SubscriptionId ==  request.SubscriptionId && a.PaymentGateway == request.PaymentGateway);
                if (PGPlan == null)
                {
                    return Result.Failure("Invalid payment gateway plan!");
                }

                var result = _mapper.Map<PGPlanDto>(PGPlan);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving payment gateway plan. Error: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
