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

namespace RubyReloaded.AuthService.Application.AjoMembers.Queries.GetAjoMembers
{
    public class GetAjoMembersByAjoId:IRequest<Result>
    {
        public int AjoId { get; set; }
    }
    public class GetAjoMembersByAjoIdHandler : IRequestHandler<GetAjoMembersByAjoId, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetAjoMembersByAjoIdHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(GetAjoMembersByAjoId request, CancellationToken cancellationToken)
        {
            try
            {
                var ajoMembers = await _context.AjoMembers.Where(x => x.AjoId == request.AjoId).ToListAsync();
                return Result.Success(ajoMembers);
            }
            catch (Exception ex)
            {
                return Result.Failure( "Ajo Member status change was not successful:"+ ex?.Message ?? ex?.InnerException.Message);
            }
        }
    }
}
