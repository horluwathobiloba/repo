using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Exceptions;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.WorkflowLevels.Queries.GetWorkflowLevels;
using Onyx.ContractService.Domain.Entities;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.WorkflowPhases.Queries.GetWorkflowPhases
{
    public class GetWorkflowLevelsDynamicQuery : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public string SearchText { get; set; }
    }


    public class GetWorkflowLevelsDynamicQueryHandler : IRequestHandler<GetWorkflowLevelsDynamicQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetWorkflowLevelsDynamicQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetWorkflowLevelsDynamicQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                var roles = await _authService.GetRolesAsync(request.AccessToken, request.OrganisationId);

                var list = (await _context.WorkflowLevels
                    .Where(a => a.OrganisationId == request.OrganisationId)
                    .Include(a => a.WorkflowPhase)
                    .ThenInclude(b => b.ContractType).ToListAsync())
                    .Where(a => request.SearchText.IsIn(a.RoleName)
                    || request.SearchText.IsIn(a.WorkflowLevelAction)
                    || request.SearchText.IsIn(a.WorkflowPhase.WorkflowSequence)
                    || request.SearchText.IsIn(a.WorkflowPhase.WorkflowUserCategory)
                    || request.SearchText.IsIn(a.WorkflowPhase.ContractType.Name));

                if (list == null)
                {
                    throw new NotFoundException(nameof(WorkflowLevel));
                }
                var result = _mapper.Map<List<WorkflowLevelDto>>(list);
                var joinList = from x in result
                               join r in roles.Entity
                               on x.RoleId equals r.Id
                               select new WorkflowLevelDto(x, r) { };
                return Result.Success(joinList); 
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving workflow phases. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }

}
