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
    public class GetAllAjoInvitationTrackers:IRequest<Result>
    {
        public int Skip { get; set; }
        public int Take { get; set; }
    }
    public class GetAllAjoInvitationTrackersHandler : IRequestHandler<GetAllAjoInvitationTrackers, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetAllAjoInvitationTrackersHandler(IApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<Result> Handle(GetAllAjoInvitationTrackers
            
            request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.AjoInvitationTrackers.Skip(request.Skip)
                                          .Take(request.Take)
                                          .ToListAsync();
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Requests was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
