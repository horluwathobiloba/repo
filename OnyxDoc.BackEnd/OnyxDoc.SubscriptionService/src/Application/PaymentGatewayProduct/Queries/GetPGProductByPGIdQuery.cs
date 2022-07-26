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


namespace OnyxDoc.SubscriptionService.Application.PGProducts.Queries
{
    public class GetPGProductByPGIdQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string PaymentGatewayProductId { get; set; }
        public string UserId { get; set; }
    }

    public class GetPGProductByPGIdQueryHandler : IRequestHandler<GetPGProductByPGIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetPGProductByPGIdQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetPGProductByPGIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var PGProduct = await _context.PGProducts.FirstOrDefaultAsync(a => a.SubscriberId == request.SubscriberId && a.PaymentGatewayProductCode == request.PaymentGatewayProductId);
                if (PGProduct == null)
                {
                    return Result.Failure("Invalid payment gateway product!");
                }

                var result = _mapper.Map<PGProductDto>(PGProduct);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving payment gateway product. Error: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
