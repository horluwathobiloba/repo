using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Products.Queries.GetProducts
{
    public class GetProductByIdQuery : IRequest<Result>
    {
        public int Id { get; set; }
        public string UserId { get; set; }
    }
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetProductByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<Result> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.Products.Where(a=>a.Id == request.Id).ToListAsync();
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Product By Id was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
