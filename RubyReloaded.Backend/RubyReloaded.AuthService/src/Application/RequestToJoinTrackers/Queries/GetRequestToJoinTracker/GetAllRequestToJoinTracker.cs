using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.RequestToJoinTrackers.Queries.GetRequestToJoinTracker
{
    public class GetAllRequestToJoinTracker:IRequest<Result>
    {
        public int Skip { get; set; }
        public int Take { get; set; }
    }

    public class GetAllRequestToJoinTrackerHandler : IRequestHandler<GetAllRequestToJoinTracker, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetAllRequestToJoinTrackerHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetAllRequestToJoinTracker request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.Cooperatives.Skip(request.Skip)
                                          .Take(request.Take)
                                          .ToListAsync();
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving requests was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
