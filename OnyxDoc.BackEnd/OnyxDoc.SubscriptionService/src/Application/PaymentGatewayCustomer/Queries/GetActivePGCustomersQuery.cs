using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.PGCustomers.Queries
{
    public class GetActivePGCustomersQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; } 
        public string UserId { get; set; }
    }

    public class GetActivePGCustomersQueryHandler : IRequestHandler<GetActivePGCustomersQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetActivePGCustomersQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetActivePGCustomersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var list = await _context.PGCustomers.Where(a => a.SubscriberId == request.SubscriberId && a.Status == Status.Active).ToListAsync();
                var result = _mapper.Map<List<PGCustomerDto>>(list);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving payment gateway customers. Error: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
