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
    public class GetWalletBeneficiaryByIdQuery : IRequest<Result>
    {
        public string UserId { get; set; }
        public int BeneficiaryId { get; set; }
    }

    public class GetWalletBeneficiaryByIdQueryHandler : IRequestHandler<GetWalletBeneficiaryByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetWalletBeneficiaryByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;

        }
        public async Task<Result> Handle(GetWalletBeneficiaryByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.WalletBeneficiaries.FirstOrDefaultAsync(x => x.Id == request.BeneficiaryId);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Beneficiary by Id was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }

}
