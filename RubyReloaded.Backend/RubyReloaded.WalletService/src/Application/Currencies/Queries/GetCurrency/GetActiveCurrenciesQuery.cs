

using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Application.Currencys.Queries.GetCurrency;
using RubyReloaded.WalletService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.CurrencyConfigurations.Queries.GetCurrency
{
    public class GetActiveCurrenciesQuery : IRequest<Result>
    {
    }

    public class GetActiveCurrenciesQueryHandler : IRequestHandler<GetActiveCurrenciesQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetActiveCurrenciesQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetActiveCurrenciesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var configurations = await _context.Currencies.Where(a => a.Status == Status.Active).ToListAsync();
                var currencyConfigurationLists = _mapper.Map<List<CurrencyListDto>>(configurations);
                return Result.Success(currencyConfigurationLists);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Error getting currencies", ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
