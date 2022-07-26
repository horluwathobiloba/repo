﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.SubscriptionService.Application.Common.Interfaces;
using RubyReloaded.SubscriptionService.Application.Common.Models;
using RubyReloaded.SubscriptionService.Application.SubscriptionPlanPricings.Queries;
using RubyReloaded.SubscriptionService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.SubscriptionService.Application.SubscriptionPlanPricings.Commands
{
    public class UpdateSubscriptionPlanPricingStatusCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int SubscriptionPlanId { get; set; }        
        public int Id { get; set; }
        public Status Status { get; set; }
        public string UserId { get; set; }
    }

    public class UpdateSubscriptionPlanPricingStatusCommandHandler : IRequestHandler<UpdateSubscriptionPlanPricingStatusCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public UpdateSubscriptionPlanPricingStatusCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(UpdateSubscriptionPlanPricingStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId ,request.UserId);

                var entity = await _context.SubscriptionPlanPricings.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId 
                && x.SubscriptionPlanId ==  request.SubscriptionPlanId && x.Id == request.Id);
                if (entity == null)
                {
                    return Result.Failure("Invalid currency!");
                }

                string message = "";
                switch (request.Status)
                {
                    case Status.Inactive:
                        entity.Status = Status.Inactive;
                        message = "Subscription pricing is now inactive!";
                        break;
                    case Status.Active:
                        entity.Status = Status.Active;
                        message = "Subscription pricing was successfully activated!";
                        break;
                    case Status.Deactivated:
                        message = "Subscription pricing was deactivated!";
                        break;
                    default:
                        break;
                }

                entity.Status = request.Status;
                entity.StatusDesc = request.Status.ToString();
                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<SubscriptionPlanPricingDto>(entity);
                return Result.Success(message, result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Subscription pricing status update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }
    }
}
