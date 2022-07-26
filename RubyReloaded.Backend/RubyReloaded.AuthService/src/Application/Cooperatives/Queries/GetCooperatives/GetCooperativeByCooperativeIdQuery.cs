using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.Cooperatives.Queries.GetCooperatives
{
    public class GetCooperativeByCooperativeIdQuery:IRequest<Result>
    {
        public int CooperativeId { get; set; }
    }

    public class GetCooperativeByCooperativeIdQueryHandler : IRequestHandler<GetCooperativeByCooperativeIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetCooperativeByCooperativeIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;

        }
        public async Task<Result> Handle(GetCooperativeByCooperativeIdQuery request, CancellationToken cancellationToken)
        {

            try
            {
                var result = await _context.Cooperatives.FirstOrDefaultAsync(x=>x.Id==request.CooperativeId);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving cooperative was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }

   
}
