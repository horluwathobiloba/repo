using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Application.Common.Models.Requests;
using RubyReloaded.WalletService.Application.Common.Models.Response;
using RubyReloaded.WalletService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Airtime.Queries
{
    public class GetAirtimePaymentMapping: IRequest<Result>
    {
        public int AirtimeCategory { get; set; }
    }

    public class GetAirtimePayementMappingHandler : IRequestHandler<GetAirtimePaymentMapping, Result>
    {
        private readonly IProvidusBankService _providus;
        private readonly IApplicationDbContext _context;

        public GetAirtimePayementMappingHandler(IProvidusBankService providus,IApplicationDbContext context)
        {
            _providus = providus;
            _context = context;
        }
        public async Task<Result> Handle(GetAirtimePaymentMapping request, CancellationToken cancellationToken)
        {
            try
            {
               // var bankService = await _context.BankServices.FirstOrDefaultAsync(x => x.BankServiceCategory == Domain.Enums.BankServiceCategory.BillPayments && x.Status == Domain.Enums.Status.Active);
                
                var mapping = await _providus.GetAirtimePaymentUIMap<List<GetFieldsMapping>>(request.AirtimeCategory);
                if (mapping is null)
                {
                    return Result.Failure("Mapping could not be found");
                }
                return Result.Success(mapping);
                
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { " Getting mapping was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }

       
    }
}

