using MediatR;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Collections.Generic;
/*using System.Data.Entity;*/
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Account.Queries.GetAccount
{
    public class GetAccountQuery : IRequest<Result>
    {
        public string UserId { get; set; }
    }

    public class GetAccountQueryHandler : IRequestHandler<GetAccountQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetAccountQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(GetAccountQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.Accounts.Include(x => x.Product).ToListAsync();
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Account was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
