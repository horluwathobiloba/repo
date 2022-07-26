﻿using AutoMapper;
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

namespace OnyxDoc.SubscriptionService.Application.PGProducts.Queries
{
    public class GetActivePGProductsQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int SubscriptionPlanId { get; set; }
        public string UserId { get; set; }
    }

    public class GetActivePGProductsQueryHandler : IRequestHandler<GetActivePGProductsQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetActivePGProductsQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetActivePGProductsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId ,request.UserId);

                var list = await _context.PGProducts.Where(a => a.SubscriberId == request.SubscriberId && a.SubscriptionPlanId == request.SubscriptionPlanId && a.Status == Status.Active).ToListAsync();
                var result = _mapper.Map<List<PGProductDto>>(list);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving payment gateway products. Error: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
