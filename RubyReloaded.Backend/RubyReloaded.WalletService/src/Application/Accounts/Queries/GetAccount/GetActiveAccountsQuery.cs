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
    public class GetActiveAccountsQuery : IRequest<Result>
    {
        public string UserId { get; set; }
    }
    public class GetActiveAccountsQueryHandler : IRequestHandler<GetActiveAccountsQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetActiveAccountsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(GetActiveAccountsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.Accounts.Include(x => x.Product).Where(x => x.Status == Domain.Enums.Status.Active)
                    .ToListAsync();
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Active Accounts was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}