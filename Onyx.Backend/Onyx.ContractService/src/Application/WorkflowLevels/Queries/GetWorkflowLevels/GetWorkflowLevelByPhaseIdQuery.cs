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

namespace Onyx.ContractService.Application.WorkflowLevels.Queries.GetWorkflowLevels
{
    public class GetWorkflowLevelByPhaseIdQuery : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public int WorkflowPhaseId { get; set; }
    }


    public class GetWorkflowLevelByWorkflowPhaseIdQueryHandler : IRequestHandler<GetWorkflowLevelByPhaseIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetWorkflowLevelByWorkflowPhaseIdQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetWorkflowLevelByPhaseIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);

                var entity = await _context.WorkflowLevels.Where(a => a.OrganisationId == request.OrganisationId
                && a.WorkflowPhaseId == request.WorkflowPhaseId)
                    .Include(a => a.WorkflowPhase)
                    .ThenInclude(b => b.ContractType)
                    .FirstOrDefaultAsync();
                if (entity == null)
                {
                    throw new NotFoundException(nameof(WorkflowLevel), request.WorkflowPhaseId);
                }
                var result = _mapper.Map<WorkflowLevelDto>(entity);
                //add the role entity
                var role = await _authService.GetRoleAsync(request.AccessToken, result.RoleId);
                result.Role = role.Entity;

                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error retrieving Workflow level by workflow phaseid {ex?.Message +" "+ ex?.InnerException?.Message}");
            }

        }
    }
}
