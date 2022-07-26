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

namespace OnyxDoc.SubscriptionService.Application.PGPrices.Queries
{
    public class GetPGPriceByPGCodeQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string PaymentGatewayPriceCode{ get; set; } 
        public string UserId { get; set; }
    }

    public class GetPGPriceByPGCodeQueryHandler : IRequestHandler<GetPGPriceByPGCodeQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetPGPriceByPGCodeQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetPGPriceByPGCodeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId ,request.UserId);

                var PGPrice = await _context.PGPrices.FirstOrDefaultAsync(a => a.SubscriberId == request.SubscriberId  && a.PaymentGatewayPriceCode == request.PaymentGatewayPriceCode);
                if (PGPrice == null)
                {
                    return Result.Failure("Invalid payment gateway price!");
                }

                var result = _mapper.Map<PGPriceDto>(PGPrice);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving payment gateway price. Error: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
