

using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Currencies.Queries.GetCurrency
{
    public class GetCurrenciesQuery : IRequest<Result>
    {
        public int Skip { get; set; }
        public int Take { get; set; }
    }


    public class GetCurrenciesQueryHandler : IRequestHandler<GetCurrenciesQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetCurrenciesQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetCurrenciesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var configurations = await _context.Currencies
                    .Skip(request.Skip)
                    .Take(request.Take).ToListAsync();
                //var currencyConfigurationLists = _mapper.Map<List<CurrencyConfigurationListDto>>(configurations);

                return Result.Success(configurations);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Error getting currency configurations", ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
