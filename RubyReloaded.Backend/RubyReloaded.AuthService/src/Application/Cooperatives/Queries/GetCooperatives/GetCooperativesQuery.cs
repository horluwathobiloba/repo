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

namespace RubyReloaded.AuthService.Application.Cooperatives.Queries.GetCooperatives
{
    public class GetCooperativesQuery : IRequest<Result>
    {
        public int Skip { get; set; }
        public int Take { get; set; }
    }
    public class GetCooperativesQueryHandler : IRequestHandler<GetCooperativesQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetCooperativesQueryHandler(IApplicationDbContext context)
        {
            _context = context;
          
        }

        public async Task<Result> Handle(GetCooperativesQuery request, CancellationToken cancellationToken)
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
                return Result.Failure(new string[] { "Retrieving cooperative was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
