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
    public class GetContractTypeInitiatorsByRoleQuery : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; } 
        public int RoleId { get; set; }
    }

    public class GetContractTypeInitiatorsByRoleQueryHandler : IRequestHandler<GetContractTypeInitiatorsByRoleQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetContractTypeInitiatorsByRoleQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetContractTypeInitiatorsByRoleQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId); 

                var entity = await _context.ContractTypeInitiators.Where(a => a.OrganisationId == request.OrganisationId 
                && a.RoleId == request.RoleId)
                    .Include(a => a.ContractType) 
                    .FirstOrDefaultAsync();
                if (entity == null)
                {
                    throw new NotFoundException(nameof(ContractTypeInitiator), request.RoleId);
                }
                var result = _mapper.Map<List<ContractTypeInitiatorDto>>(entity);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error retrieving Contract type Initiator by role {ex?.Message +" "+ ex?.InnerException?.Message}");
            }

        }
    }
}
