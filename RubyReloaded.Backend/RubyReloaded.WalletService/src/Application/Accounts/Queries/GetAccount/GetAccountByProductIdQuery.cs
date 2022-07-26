using MediatR;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Account.Queries.GetAccount
{
    public class GetAccountByProductIdQuery : IRequest<Result>
    {
        public string UserId { get; set; }
        public int ProductId { get; set; }
    }

    public class GetAccountByProductIdQueryHandler : IRequestHandler<GetAccountByProductIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetAccountByProductIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(GetAccountByProductIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                //TODO: Set up AdminAccess where needed
                var result = await _context.Accounts
                    .Include(x => x.Product)
                    .Where(a=>a.ProductId == request.ProductId)
                    .ToListAsync();
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Account by Product Id was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
