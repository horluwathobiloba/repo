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

namespace RubyReloaded.AuthService.Application.CooperativeInvitationTrackers.Queries.GetCooperativeInvitationTracker
{
    public class GetAllCooperativeInvitatonTrackerByCooperativeId:IRequest<Result>
    {
        public int CooperativeId { get; set; }
    }
    public class GetAllCooperativeInvitatonTrackerByCooperativeIdHandler : IRequestHandler<GetAllCooperativeInvitatonTrackerByCooperativeId, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetAllCooperativeInvitatonTrackerByCooperativeIdHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetAllCooperativeInvitatonTrackerByCooperativeId request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.CooperativeInvitationTrackers.Where(x =>x.CooperativeId == request.CooperativeId).ToListAsync();
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure( "Retrieving requests was not successful"+ex?.Message ?? ex?.InnerException.Message);
            }
        }
    }
}
