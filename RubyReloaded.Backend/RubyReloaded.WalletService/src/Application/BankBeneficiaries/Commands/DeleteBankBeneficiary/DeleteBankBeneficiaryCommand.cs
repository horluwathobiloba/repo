using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.BankBeneficiaries.Commands.DeleteBankBeneficiary
{
    public class DeleteBankBeneficiaryCommand:IRequest<Result>
    {
        public int Id { get; set; }
    }
    public class DeleteBankBeneficiaryCommandHandler : IRequestHandler<DeleteBankBeneficiaryCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        public DeleteBankBeneficiaryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(DeleteBankBeneficiaryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.BankBeneficiaries.FirstOrDefaultAsync(x => x.Id == request.Id);
                entity.Status = Domain.Enums.Status.Deactivated;
                _context.BankBeneficiaries.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Deletion Successful");
            }
            catch (Exception ex)
            {
                return Result.Failure("Deletion was not successful" + ex?.Message ?? ex?.InnerException.Message);
            }
        }
    }

}
