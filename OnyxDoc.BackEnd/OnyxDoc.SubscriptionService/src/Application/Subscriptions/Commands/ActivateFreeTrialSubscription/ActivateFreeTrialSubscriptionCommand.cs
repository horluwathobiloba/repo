﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Application.SubscriptionPlans.Queries;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.Subscriptions.Commands
{
    public class ActivateFreeTrialSubscription : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int Id { get; set; }
        public string UserId { get; set; }
    }

    public class ActivateFreeTrialSubscriptionHandler : IRequestHandler<ActivateFreeTrialSubscription, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        private readonly ISubscriberService _subscriberService;

        public ActivateFreeTrialSubscriptionHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService, IConfiguration configuration, ISubscriberService subscriberService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
            _configuration = configuration;
            _subscriberService = subscriberService;
        }

        public async Task<Result> Handle(ActivateFreeTrialSubscription request, CancellationToken cancellationToken)
        {
            try
            {
                var command = new UpdateSubscriptionStatusCommand()
                {
                    AccessToken = request.AccessToken,
                    Id = request.Id,
                    SubscriptionStatus = SubscriptionStatus.FreeTrial,
                    SubscriberId = request.SubscriberId,
                    UserId = request.UserId
                };
                var result = await new UpdateSubscriptionStatusCommandHandler(_context, _mapper, _authService, _configuration, _subscriberService).Handle(command, cancellationToken);

                return result;
            }
            catch (Exception ex)
            {
                return Result.Failure($"Free trial subscription activation failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }
    }
}
