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
    public class GetRequestTrackersByCooperativeId:IRequest<Result>
    {
        public int CooperativeId { get; set; }
    }

    public class GetRequestTrackersByCooperativeIdHandler : IRequestHandler<GetRequestTrackersByCooperativeId, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetRequestTrackersByCooperativeIdHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(GetRequestTrackersByCooperativeId request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.RequestToJoinTrackers.Where(x => x.CooperativeId == request.CooperativeId).ToListAsync();
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure( "Retrieving request was not successful"+ ex?.Message ?? ex?.InnerException.Message);
            }
        }
    }
}
