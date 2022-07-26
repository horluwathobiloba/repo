using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Exceptions;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ContractDuration.Queries.GetAllCurrencies
{
    public class GetAllCurrencies : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
    }
    public class GetAllCurrenciesHandler : IRequestHandler<GetAllCurrencies, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService; 

        public GetAllCurrenciesHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetAllCurrencies request, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                if (!response)
                {
                    return Result.Failure($"Invalid organisation and user details");
                }
                if (request.OrganisationId <= 0)
                {
                    return Result.Failure($"Organisation must be specified.");
                }
                var currencies = await _context.CurrencyConfigurations.Where(a=>a.OrganisationId == request.OrganisationId).ToListAsync();
                return Result.Success(currencies);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving currencies. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }

}
