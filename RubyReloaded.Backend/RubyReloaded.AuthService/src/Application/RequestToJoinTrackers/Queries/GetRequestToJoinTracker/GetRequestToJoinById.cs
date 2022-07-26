using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.RequestToJoinTrackers.Queries.GetRequestToJoinTracker
{
    public class GetRequestToJoinById:IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class GetRequestToJoinByIdQueryHandler : IRequestHandler<GetRequestToJoinById, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetRequestToJoinByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;

        }
        public async Task<Result> Handle(GetRequestToJoinById request, CancellationToken cancellationToken)
        {

            try
            {
                var result = await _context.RequestToJoinTrackers.FirstOrDefaultAsync(x => x.Id == request.Id);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving request was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
