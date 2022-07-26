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

namespace RubyReloaded.AuthService.Application.AjoInvitationTrackers.Queries.GetAjoInvitationTracker
{
    public class GetAjoInvitationTrackerByAjoId:IRequest<Result>
    {
        public int AjoId { get; set; }
    }

    public class GetAjoInvitationTrackerByAjoIdHandler : IRequestHandler<GetAjoInvitationTrackerByAjoId, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetAjoInvitationTrackerByAjoIdHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetAjoInvitationTrackerByAjoId request, CancellationToken cancellationToken)
        {
            try
            { 
                var result = await _context.AjoInvitationTrackers.Where(x => x.AjoId == request.AjoId).ToListAsync();
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving requests was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
