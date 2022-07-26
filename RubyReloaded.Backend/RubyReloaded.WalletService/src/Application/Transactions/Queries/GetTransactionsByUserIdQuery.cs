using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.s.Queries
{
    public class GetTransactionByUserIdQuery : IRequest<Result>
    {
        public string UserId { get; set; }
      
    }

    public class GetTransactionByUserIdQueryHandler : IRequestHandler<GetTransactionByUserIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetTransactionByUserIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;

        }
        public async Task<Result> Handle(GetTransactionByUserIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.Transactions.Where(x=> x.CreatedBy == request.UserId).ToListAsync();
                return Result.Success( result);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving  Transaction was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }


}
