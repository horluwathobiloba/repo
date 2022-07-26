using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Exceptions;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Domain.Entities;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ContractTypeInitiators.Queries.GetContractTypeInitiators
{
    public class GetContractTypeInitiatorsDynamicQuery : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public string SearchText { get; set; }
    }


    public class GetContractTypeInitiatorsDynamicQueryHandler : IRequestHandler<GetContractTypeInitiatorsDynamicQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetContractTypeInitiatorsDynamicQueryHandler(IApplicationDbContext context, IMapper mapper,IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetContractTypeInitiatorsDynamicQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                var list = await _context.ContractTypeInitiators
                    .Include(a => a.ContractType)
                    .Where(a => a.OrganisationId == request.OrganisationId
                    && (request.SearchText.ToLower() == a.RoleName.ToLower() || request.SearchText.ToLower() == a.ContractType.Name.ToLower()))
                    .ToListAsync();

                if (list == null)
                {
                    throw new NotFoundException(nameof(ContractTypeInitiator));
                }
                var result = _mapper.Map<List<ContractTypeInitiatorDto>>(list);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving workflow phases. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }

}
