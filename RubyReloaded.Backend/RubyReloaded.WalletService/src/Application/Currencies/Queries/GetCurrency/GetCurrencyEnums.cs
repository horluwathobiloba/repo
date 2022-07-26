using MediatR;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Domain.Enums;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Currencies.Queries.GetCurrency
{
    public class GetCurrencyEnums:IRequest<Result>
    {
    }


    public class GetCurrencyEnumsHandler : IRequestHandler<GetCurrencyEnums, Result>
    {
        private readonly IApplicationDbContext _context;
      
        public GetCurrencyEnumsHandler(IApplicationDbContext context)
        {
            _context = context;
           
           
        }

        public async Task<Result> Handle(GetCurrencyEnums request, CancellationToken cancellationToken)
        {
            try
            {
                return Result.Success(Enum.GetNames(typeof(CurrencyCode)).ToList());
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving currency enums. Error: {ex?.Message + " " + ex?.InnerException.Message}");
            }
        }
    }
}
