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

namespace OnyxDoc.SubscriptionService.Application.PGCustomers.Queries
{
    public class GetPGCustomerByPGIdQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int PaymentGatewayCustomerId { get; set; } 
        public string UserId { get; set; }
    }

    public class GetPGCustomerByPGIdQueryHandler : IRequestHandler<GetPGCustomerByPGIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetPGCustomerByPGIdQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetPGCustomerByPGIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId ,request.UserId);

                var PGCustomer = await _context.PGCustomers.FirstOrDefaultAsync(a => a.SubscriberId == request.SubscriberId  && a.PaymentGatewayCustomerId == request.PaymentGatewayCustomerId);
                if (PGCustomer == null)
                {
                    return Result.Failure("Invalid payment gateway customer!");
                }

                var result = _mapper.Map<PGCustomerDto>(PGCustomer);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving payment gateway customer. Error: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
