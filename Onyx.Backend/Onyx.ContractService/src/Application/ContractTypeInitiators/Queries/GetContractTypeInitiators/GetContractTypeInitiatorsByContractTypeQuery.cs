using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Exceptions;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ContractTypeInitiators.Queries.GetContractTypeInitiators
{
    public class GetContractTypeInitiatorsByContractTypeQuery : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public int ContractTypeId { get; set; }
    }


    public class GetContractTypeInitiatorsByContractTypeQueryHandler : IRequestHandler<GetContractTypeInitiatorsByContractTypeQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetContractTypeInitiatorsByContractTypeQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetContractTypeInitiatorsByContractTypeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);

                var entity = await _context.ContractTypeInitiators.Where(a => a.OrganisationId == request.OrganisationId
                && a.ContractTypeId == request.ContractTypeId)
                    .Include(a => a.ContractType)
                    .FirstOrDefaultAsync();
                if (entity == null)
                {
                    throw new NotFoundException(nameof(ContractTypeInitiator), request.ContractTypeId);
                }
                var result = _mapper.Map<List<ContractTypeInitiatorDto>>(entity);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error retrieving Contract type Initiator by contract type id {ex?.Message +" "+ ex?.InnerException?.Message}");
            }

        }
    }
}
