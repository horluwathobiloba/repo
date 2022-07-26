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

namespace Onyx.ContractService.Application.WorkflowPhases.Queries.GetWorkflowPhases
{
    public class GetWorkflowPhasesQuery : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
    }

    public class GetWorkflowPhasesQueryHandler : IRequestHandler<GetWorkflowPhasesQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        public GetWorkflowPhasesQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetWorkflowPhasesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                if (request.OrganisationId <= 0)
                {
                    return Result.Failure($"Organisation must be specified.");
                }
                var list = await _context.WorkflowPhases.Where(a => a.OrganisationId == request.OrganisationId).ToListAsync();
                if (list == null)
                {
                    throw new NotFoundException(nameof(WorkflowPhase));
                }
                var result = _mapper.Map<List<WorkflowPhaseDto>>(list);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error retrieving workflow phases. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
