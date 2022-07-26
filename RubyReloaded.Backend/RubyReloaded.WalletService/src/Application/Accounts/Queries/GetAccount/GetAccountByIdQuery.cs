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
    public class GetAccountByIdQuery : IRequest<Result>
    {
        public string UserId { get; set; }
        public int AccountId { get; set; }
    }
    public class GetAccountByIdQueryHandler : IRequestHandler<GetAccountByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetAccountByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.Accounts
                    .Include(x=>x.Product).
                    FirstOrDefaultAsync(x => x.Id == request.AccountId);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Account was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}