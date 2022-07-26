using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.WalletBeneficiaries.Queries.GetWalletBeneficiaries
{
    public class GetWalletBeneficiariesByUserId:IRequest<Result>
    {
        public string UserId { get; set; }
    }


    public class GetWalletBeneficiariesByUserIdHandler : IRequestHandler<GetWalletBeneficiariesByUserId, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetWalletBeneficiariesByUserIdHandler(IApplicationDbContext context)
        {
            _context = context;

        }
        public async Task<Result> Handle(GetWalletBeneficiariesByUserId request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.WalletBeneficiaries
                    .Where(x => x.UserId == request.UserId)
                    .ToListAsync();
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Beneficiaries by UserId was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }

}
