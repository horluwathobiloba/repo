using MediatR;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.CurrencyConfigurations.Queries.GetCurrencyConfigurations
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
