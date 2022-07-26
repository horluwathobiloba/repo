using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.CurrencyConfigurations.Queries.GetCurrencyConfigurations
{
    public class GetCurrencyConfigurationsQuery : IRequest<Result>
    {
        public int Skip { get; set; }
        public int Take { get; set; }
    }


    public class GetCurrencyConfigurationsQueryHandler : IRequestHandler<GetCurrencyConfigurationsQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetCurrencyConfigurationsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetCurrencyConfigurationsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var configurations = await _context.CurrencyConfigurations
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
