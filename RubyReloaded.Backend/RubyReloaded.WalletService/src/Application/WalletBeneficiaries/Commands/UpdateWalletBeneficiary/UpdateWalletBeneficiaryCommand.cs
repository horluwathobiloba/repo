using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.WalletBeneficiaries.Commands.UpdateWalletBeneficiary
{
    public class UpdateWalletBeneficiaryCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public int WalletId { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string LoggedInUser { get; set; }
    }
    public class UpdateWalletBeneficiaryCommandHandler : IRequestHandler<UpdateWalletBeneficiaryCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        public UpdateWalletBeneficiaryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(UpdateWalletBeneficiaryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.WalletBeneficiaries.FirstOrDefaultAsync(x => x.Id == request.Id);
                if (entity == null || entity.UserId != request.LoggedInUser)
                {
                    return Result.Failure(new string[] { "Beneficiary Id is invalid " });
                }

                entity.Username = request.Username;
                entity.WalletId = request.WalletId;
                entity.Email = request.Email;
                entity.LastModifiedDate = DateTime.Now;
                _context.WalletBeneficiaries.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);
                //  await _context.CommitTransactionAsync();
                return Result.Success(entity);
            }
            catch (Exception ex)
            {
                // _context.RollbackTransaction();
                return Result.Failure(new string[] { "Beneficiaries  Update was not successful ", ex?.Message ?? ex?.InnerException.Message });
            }

        }
    }
}
