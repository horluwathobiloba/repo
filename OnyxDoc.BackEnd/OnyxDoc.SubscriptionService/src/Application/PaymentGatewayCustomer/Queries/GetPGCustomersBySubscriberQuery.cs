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
    public class GetPGCustomersBySubscriberQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; } 
        public string UserId { get; set; }
    }
    public class GetPGCustomersBySubscriberQueryHandler : IRequestHandler<GetPGCustomersBySubscriberQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;
        public GetPGCustomersBySubscriberQueryHandler(IApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        public async Task<Result> Handle(GetPGCustomersBySubscriberQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId ,request.UserId);

                var entity = await _context.PGCustomers.Where(x => x.SubscriberId == request.SubscriberId ).ToListAsync();

                if (entity == null || entity.Count == 0)
                {
                    return Result.Failure("No record found");
                }
                return Result.Success($"{entity.Count}(s) found.", entity);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Get payment gateway customers failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
