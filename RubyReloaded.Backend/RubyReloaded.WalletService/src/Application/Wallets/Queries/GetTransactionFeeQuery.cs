using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Wallets.Queries
{
    public class GetTransactionFeeQuery : IRequest<Result>
    {
        public decimal Amount { get; set; }
        public BankServiceCategory BankServiceCategory { get; set; }
    }


    public class GetTransactionFeeQueryHandler : IRequestHandler<GetTransactionFeeQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetTransactionFeeQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(GetTransactionFeeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var bankService = await _context.BankServices.FirstOrDefaultAsync(x => x.BankServiceCategory == request.BankServiceCategory&&x.Status==Status.Active);
                if (bankService is null)
                {
                    return Result.Failure("There is no bank service avaliable for this category");
                }
                var transactionfee = GetTransactionFee(bankService, request.Amount);
                return Result.Success(transactionfee);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Transaction fee was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
            
        }

        private decimal GetTransactionFee(Domain.Entities.BankService bankService, decimal amount)
        {
            if (bankService.TransactionFeeType == FeeType.Percentage)
            {
                var percentageValue = bankService.TransactionFee / 100;
                var realValue = percentageValue * amount;
                return realValue;
            }
            return bankService.TransactionFee;
        }
    }
}
