using MediatR;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.VirtualAccount.Query.GetVirtualAccount
{
    public class GetVirtualAccountQuery : IRequest<Result>
    {
        public string UserId { get; set; }
    }

    public class VirtualAccountQueryHandler : IRequestHandler<GetVirtualAccountQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        public VirtualAccountQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(GetVirtualAccountQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.VirtualAccounts.ToListAsync();
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Virtual Accounts was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
