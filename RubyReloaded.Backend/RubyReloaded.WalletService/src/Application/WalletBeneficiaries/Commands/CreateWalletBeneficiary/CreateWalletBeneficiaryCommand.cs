using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Domain.Entities;
using System;
using System.Collections.Generic;

using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.WalletBeneficiaries.Commands.CreateWalletBeneficiary
{
    public class CreateWalletBeneficiaryCommand:IRequest<Result>
    {
        public int WalletId { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string LoggedInUser { get; set; }
    }

    public class CreateWalletBeneficiaryCommandHandler : IRequestHandler<CreateWalletBeneficiaryCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public CreateWalletBeneficiaryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(CreateWalletBeneficiaryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //check if any PaymentChannel for the customer is active or if the name already exists.. If yes, then return a failure response else go ahead and create the PaymentChannel
                var exists = await _context.WalletBeneficiaries.FirstOrDefaultAsync(a=>a.Username==request.Username&&a.WalletId==request.WalletId);
                if (exists?.Status==Domain.Enums.Status.Active)
                {
                    return Result.Failure("Create new Beneficiary failed because a beneficiary name already exists. Please enter a new beneficiary  name to continue.");
                }
                if (exists?.Status == Domain.Enums.Status.Deactivated)
                {
                    exists.Status = Domain.Enums.Status.Active;
                    _context.WalletBeneficiaries.Update(exists);
                   await _context.SaveChangesAsync(cancellationToken);
                    return Result.Success(exists);
                }
                var entity = new WalletBeneficiary
                {
                    Username = request.Username,
                    WalletId = request.WalletId,
                    CreatedDate = DateTime.Now,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    Status = Domain.Enums.Status.Active,
                    StatusDesc = Domain.Enums.Status.Active.ToString(),
                    UserId=request.LoggedInUser
                };
                _context.WalletBeneficiaries.Add(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Beneficiary created successfully!", entity);
            }
            catch (Exception ex)
            {
                //return Result.Failure(new string[] { "Beneficiary creation failed!", ex?.Message ?? ex?.InnerException.Message });
                return Result.Failure("Beneficiary creation failed!" + ex?.Message ?? ex?.InnerException.Message);
            }
        }
        public async Task<Result> CreateBeneficiary(WalletBeneficiary walletBeneficiary, CancellationToken cancellationToken)
        {
            try
            {
                _context.WalletBeneficiaries.Add(walletBeneficiary);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Beneficiary created successfully!", walletBeneficiary);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Beneficiary creation failed!", ex?.Message ?? ex?.InnerException.Message });
            }

        }
    }
}

