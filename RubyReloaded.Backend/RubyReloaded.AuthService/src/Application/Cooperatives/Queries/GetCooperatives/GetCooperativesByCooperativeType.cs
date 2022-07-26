using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.Cooperatives.Queries.GetCooperatives
{
    public class GetCooperativesByCooperativeType:IRequest<Result>
    {
        public CooperativeType CooperativeType { get; set; }
    }

    public class GetCooperativesByCooperativeTypeHandler : IRequestHandler<GetCooperativesByCooperativeType, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetCooperativesByCooperativeTypeHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(GetCooperativesByCooperativeType request, CancellationToken cancellationToken)
        {
            try
            {
                var cooperatives = await _context.Cooperatives.Where(x => x.CooperativeType == request.CooperativeType).ToListAsync();
                return Result.Success(cooperatives);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving cooperative was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
