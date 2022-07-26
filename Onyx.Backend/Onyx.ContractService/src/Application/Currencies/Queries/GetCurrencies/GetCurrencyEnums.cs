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

namespace Onyx.ContractService.Application.ContractDuration.Queries.GetCurrencies
{
    public class GetCurrencyEnums : AuthToken, IRequest<Result>
    {
        
    }
    public class GetCurrencyEnumsHandler : IRequestHandler<GetCurrencyEnums, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetCurrencyEnumsHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetCurrencyEnums request, CancellationToken cancellationToken)
        {
            try
            {
                return Result.Success(Enum.GetNames(typeof(CurrencySymbol)).ToList());
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving currency enums. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }

}
