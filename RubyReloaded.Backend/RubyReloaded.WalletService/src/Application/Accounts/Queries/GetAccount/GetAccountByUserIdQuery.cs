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
    public class GetAccountByUserIdQuery : IRequest<Result>
    {
        public string UserId { get; set; }
    }

    public class GetAccountByUserIdQueryHandler : IRequestHandler<GetAccountByUserIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetAccountByUserIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(GetAccountByUserIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.Accounts
                    .Include(x => x.Product)
                    .Where(a => a.UserId == request.UserId)
                   
                    .ToListAsync();
                //var result = await _context.Accounts.Where(a => a.UserId == request.UserId).ToListAsync();
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Account by UserId was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
