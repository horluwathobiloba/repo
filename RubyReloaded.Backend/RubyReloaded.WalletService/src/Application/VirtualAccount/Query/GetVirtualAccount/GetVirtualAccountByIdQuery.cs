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
    public class GetVirtualAccountByIdQuery : IRequest<Result>
    {
        public string UserId { get; set; }
        public int VirtualAccountId { get; set; }
    }

    public class GetVirtualAccountByIdQueryHandler : IRequestHandler<GetVirtualAccountByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetVirtualAccountByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(GetVirtualAccountByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.VirtualAccounts.FirstOrDefaultAsync(x => x.Id == request.VirtualAccountId);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Virtual Account was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
