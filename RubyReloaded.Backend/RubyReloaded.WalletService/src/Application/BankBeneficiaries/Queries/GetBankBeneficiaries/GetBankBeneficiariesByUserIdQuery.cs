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

namespace RubyReloaded.WalletService.Application.BankBeneficiaries.Queries.GetBankBeneficiaries
{
    public class GetBankBeneficiariesByUserIdQuery : IRequest<Result>
    {
        public string UserId { get; set; }
    }

    public class GetBankBeneficiariesByUserIdQueryHandler : IRequestHandler<GetBankBeneficiariesByUserIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetBankBeneficiariesByUserIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;

        }
        public async Task<Result> Handle(GetBankBeneficiariesByUserIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.BankBeneficiaries
                    .Where(x => x.UserId == request.UserId&&x.Status==Domain.Enums.Status.Active)
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
