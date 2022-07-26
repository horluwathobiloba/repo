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
    public class GetAccountByAccountNumberQuery : IRequest<Result>
    {
        public string AccountNumber { get; set; }
    }

    public class GetAccountByAccountNumberQueryHandler : IRequestHandler<GetAccountByAccountNumberQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetAccountByAccountNumberQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(GetAccountByAccountNumberQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.Accounts
                    .Include(x => x.Product)
                    .FirstOrDefaultAsync(a => a.AccountNumber == request.AccountNumber);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Account by AccountNumber was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
