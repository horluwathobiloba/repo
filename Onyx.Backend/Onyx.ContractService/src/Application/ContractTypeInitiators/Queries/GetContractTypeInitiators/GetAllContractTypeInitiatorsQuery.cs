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
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ContractTypeInitiators.Queries.GetContractTypeInitiators
{
    public class GetContractTypeInitiatorsQuery : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
    }

    public class GetContractTypeInitiatorsQueryHandler : IRequestHandler<GetContractTypeInitiatorsQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetContractTypeInitiatorsQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper; 
            _authService = authService;
        }

        public async Task<Result> Handle(GetContractTypeInitiatorsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);

                var list = await _context.ContractTypeInitiators.Where(a => a.OrganisationId == request.OrganisationId)
                    .Include(a => a.ContractType) 
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
                return Result.Failure($"Error retrieving Contract type Initiators. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
