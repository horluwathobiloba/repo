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

namespace Onyx.ContractService.Application.WorkflowPhases.Queries.GetWorkflowPhases
{
    public class GetWorkflowPhaseByContractIdQuery : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public int ContractTypeId { get; set; }
    }

    public class GetWorkflowPhaseByContractIdQueryHandler : IRequestHandler<GetWorkflowPhaseByContractIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetWorkflowPhaseByContractIdQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetWorkflowPhaseByContractIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.OrganisationId <= 0)
                {
                    return Result.Failure($"Organisation must be specified.");
                }
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                
                var entity = await _context.WorkflowPhases.Where(a => a.OrganisationId == request.OrganisationId
                && a.ContractTypeId == request.ContractTypeId).ToListAsync();

                if (entity == null)
                {
                    throw new NotFoundException(nameof(WorkflowPhase), request.ContractTypeId);
                }
                var result = _mapper.Map<List<WorkflowPhaseDto>>(entity);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error retrieving workflow phase by contract id {ex?.Message +" "+ ex?.InnerException?.Message}");
            }

        }
    }
}
