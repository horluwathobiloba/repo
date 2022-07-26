using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.WalletBeneficiaries.Queries.GetWalletBeneficiaries
{
    public class GetWalletBeneficiariesQuery : IRequest<Result>
    {
        public string UserId { get; set; }
    }

    public class GetWalletBeneficiariesQueryHandler : IRequestHandler<GetWalletBeneficiariesQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetWalletBeneficiariesQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetWalletBeneficiariesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.WalletBeneficiaries.ToListAsync();
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Products was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }

}
