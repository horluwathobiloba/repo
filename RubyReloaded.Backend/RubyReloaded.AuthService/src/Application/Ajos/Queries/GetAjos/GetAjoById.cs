using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.Ajos.Queries.GetAjos
{
    public class GetAjoById:IRequest<Result>
    {
        public int Id { get; set; }
    }
    public class GetAjoByIdHandler : IRequestHandler<GetAjoById, Result>
    {

        private readonly IApplicationDbContext _context;
        public GetAjoByIdHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(GetAjoById request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.Ajos.FirstOrDefaultAsync(x => x.Id == request.Id);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving ajo was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
