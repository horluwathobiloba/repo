﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.SubscriptionService.Application.Common.Interfaces;
using RubyReloaded.SubscriptionService.Application.Common.Models;
using RubyReloaded.SubscriptionService.Application.PaymentChannels.Queries;
using RubyReloaded.SubscriptionService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.SubscriptionService.Application.PaymentChannels.Commands
{
    public class DeactivatePaymentChannelCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int Id { get; set; }
        public string UserId { get; set; }
    }

    public class DeactivatePaymentChannelCommandHandler : IRequestHandler<DeactivatePaymentChannelCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public DeactivatePaymentChannelCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(DeactivatePaymentChannelCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var command = new UpdatePaymentChannelStatusCommand()
                {
                    AccessToken = request.AccessToken,
                    Id = request.Id,
                    Status = Status.Deactivated,
                    SubscriberId = request.SubscriberId,
                    UserId = request.UserId
                };
                var result = await new UpdatePaymentChannelStatusCommandHandler( _context, _mapper, _authService).Handle(command, cancellationToken);

                return result;
            }
            catch (Exception ex)
            {
                return Result.Failure($"PaymentChannel deactivation failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }
    }
}
