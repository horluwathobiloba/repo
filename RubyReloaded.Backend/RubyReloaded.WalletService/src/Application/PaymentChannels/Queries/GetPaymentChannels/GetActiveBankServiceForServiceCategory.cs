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

namespace RubyReloaded.WalletService.Application.PaymentChannels.Queries.GetPaymentChannels
{
    public class GetActiveBankServiceForServiceCategory:IRequest<Result>
    {
        public BankServiceCategory BankServiceCategory { get; set; }
    }

    public class GetActiveBankServiceForServiceCategoryHandler : IRequestHandler<GetActiveBankServiceForServiceCategory, Result>
    {
        private readonly IApplicationDbContext _context;

        public GetActiveBankServiceForServiceCategoryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(GetActiveBankServiceForServiceCategory request, CancellationToken cancellationToken)
        {
            try
            {
                var bankService = await _context.BankServices.FirstOrDefaultAsync(x => x.BankServiceCategory == request.BankServiceCategory && x.Status == Status.Active);
                if (bankService is null)
                {
                    return Result.Success("There is no bank service avaliable for this category",true);
                }
                return Result.Success("There is a bank service avaliable for this category", false);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving bank service was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
