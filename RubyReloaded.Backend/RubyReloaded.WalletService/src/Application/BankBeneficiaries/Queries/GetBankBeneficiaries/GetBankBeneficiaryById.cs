using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.BankBeneficiaries.Queries.GetBankBeneficiaries
{
    public class GetBankBeneficiaryById : IRequest<Result>
    {
        public string UserId { get; set; }
        public int BeneficiaryId { get; set; }
    }


    public class GetBankBeneficiaryByIdQueryHandler : IRequestHandler<GetBankBeneficiaryById, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetBankBeneficiaryByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;

        }
        public async Task<Result> Handle(GetBankBeneficiaryById request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.BankBeneficiaries.FirstOrDefaultAsync(x => x.Id == request.BeneficiaryId);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Beneficiary by Id was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }


}
