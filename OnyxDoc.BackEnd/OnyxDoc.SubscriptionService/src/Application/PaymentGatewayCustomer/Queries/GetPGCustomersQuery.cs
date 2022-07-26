﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.PGCustomers.Queries
{
    public class GetPGCustomersQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string UserId { get; set; }
    }

    public class GetPGCustomersQueryHandler : IRequestHandler<GetPGCustomersQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetPGCustomersQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetPGCustomersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var list = await _context.PGCustomers.Where(a => a.SubscriberId == request.SubscriberId).ToListAsync();
                var result = _mapper.Map<List<PGCustomerDto>>(list);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving payment gateway customer. Error: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
