using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.SubscriptionService.Application.Common.Exceptions;
using RubyReloaded.SubscriptionService.Application.Common.Interfaces;
using RubyReloaded.SubscriptionService.Application.Common.Models;
using RubyReloaded.SubscriptionService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.SubscriptionService.Application.PaymentChannels.Queries
{
    public class GetPaymentChannelByIdQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int Id { get; set; }
        public string UserId { get; set; }
    }

    public class GetPaymentChannelByIdQueryHandler : IRequestHandler<GetPaymentChannelByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetPaymentChannelByIdQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetPaymentChannelByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var PaymentChannel = await _context.PaymentChannels.FirstOrDefaultAsync(a => a.SubscriberId == request.SubscriberId && a.Id == request.Id);
                if (PaymentChannel == null)
                {
                    return Result.Failure("Invalid payment channel specified");
                }

                var result = _mapper.Map<PaymentChannelDto>(PaymentChannel);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving payment channel. Error: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
