using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.BankBeneficiaries.Commands.UpdateBankBeneficiaries
{
    public class UpdateBankBeneficiariesCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string LoggedInUser { get; set; }
    }

    public class UpdateBankBeneficiariesCommandHandler : IRequestHandler<UpdateBankBeneficiariesCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        public UpdateBankBeneficiariesCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(UpdateBankBeneficiariesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.BankBeneficiaries.FirstOrDefaultAsync(x => x.Id == request.Id);
                if (entity == null || entity.UserId != request.LoggedInUser)
                {
                    return Result.Failure(new string[] { "Beneficiary Id is invalid " });
                }

                entity.AccountNumber = request.AccountNumber;
                entity.BankName = request.BankName;
                entity.Email = request.Email;
                entity.PhoneNumber = request.PhoneNumber;
                entity.LastModifiedDate = DateTime.Now;
                _context.BankBeneficiaries.Update(entity);
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
