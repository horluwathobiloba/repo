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

namespace RubyReloaded.AuthService.Application.Ajos.Queries
{
    public class GetAllAjos:IRequest<Result>
    {
        public int Skip { get; set; }
        public int Take { get; set; }
    }

    public class GetAllAjosHandler : IRequestHandler<GetAllAjos, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetAllAjosHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(GetAllAjos request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.Ajos.Skip(request.Skip)
                                          .Take(request.Take)
                                          .ToListAsync();
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving ajos was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
