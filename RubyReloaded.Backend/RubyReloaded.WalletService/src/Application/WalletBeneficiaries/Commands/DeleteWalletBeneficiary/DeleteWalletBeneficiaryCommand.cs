using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.WalletBeneficiaries.Commands.DeleteWalletBeneficiary
{
    public class DeleteWalletBeneficiaryCommand:IRequest<Result>
    {
        public int Id { get; set; }
    }
    public class DeleteWalletBeneficiaryCommandHandler : IRequestHandler<DeleteWalletBeneficiaryCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        public DeleteWalletBeneficiaryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(DeleteWalletBeneficiaryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.WalletBeneficiaries.FirstOrDefaultAsync(x => x.Id == request.Id);
                entity.Status = Domain.Enums.Status.Deactivated;
                _context.WalletBeneficiaries.Update(entity);
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
