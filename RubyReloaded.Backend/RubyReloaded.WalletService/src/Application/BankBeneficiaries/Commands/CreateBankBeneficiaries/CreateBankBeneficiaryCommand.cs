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

namespace RubyReloaded.WalletService.Application.BankBeneficiaries.Commands.CreateBankBeneficiaries
{
    public class CreateBankBeneficiaryCommand:IRequest<Result>
    {
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string LoggedInUser { get; set; }
        public string Name { get; set; }


    }
    public class CreateBankBeneficiaryCommandHandler : IRequestHandler<CreateBankBeneficiaryCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public CreateBankBeneficiaryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(CreateBankBeneficiaryCommand request, CancellationToken cancellationToken)
        {
           
                //check if any PaymentChannel for the customer is active or if the name already exists.. If yes, then return a failure response else go ahead and create the PaymentChannel
                var exists = await _context.BankBeneficiaries
                    .FirstOrDefaultAsync(a => a.AccountNumber == request.AccountNumber
                && a.BankName == request.BankName);
            //if (exists!=null)
            //{

            //}
                if (exists?.Status == Domain.Enums.Status.Active)
                {
                    return Result.Failure("Create new Beneficiary failed because a beneficiary name already exists. Please enter a new beneficiary  name to continue.");
                }
                if (exists?.Status == Domain.Enums.Status.Deactivated)
                {
                    exists.Status = Domain.Enums.Status.Active;
                    _context.BankBeneficiaries.Update(exists);
                    await _context.SaveChangesAsync(cancellationToken);
                    return Result.Success(exists);
                }

                var entity = new BankBeneficiary
                {
                    AccountNumber=request.AccountNumber,
                    BankName=request.BankName,
                    UserId=request.LoggedInUser,
                    CreatedDate = DateTime.Now,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    Status = Domain.Enums.Status.Active,
                    StatusDesc = Domain.Enums.Status.Active.ToString(),
                    Name=request.Name
                };
               var result= await this.CreateBeneficiary(entity, cancellationToken);
                return result;
        }

        public async Task<Result>CreateBeneficiary(BankBeneficiary bankBeneficiary, CancellationToken cancellationToken)
        {
            try
            {
                _context.BankBeneficiaries.Add(bankBeneficiary);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Beneficiary created successfully!", bankBeneficiary);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Beneficiary creation failed!", ex?.Message ?? ex?.InnerException.Message });
            }

        }
    }
}
