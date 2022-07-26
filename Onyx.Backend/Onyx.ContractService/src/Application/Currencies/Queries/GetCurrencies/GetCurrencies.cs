using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.Currencies.Queries.GetCurrencies
{
    public class GetCurrencies : IRequest<Result>
    {
       
    }
    public class GetCurrenciesHandler : IRequestHandler<GetCurrencies, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetCurrenciesHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetCurrencies request, CancellationToken cancellationToken)
        {
            try
            {   
                var currencies = await _context.CurrencyConfigurations.ToListAsync();
                return Result.Success(currencies);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving currencies. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
