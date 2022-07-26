using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Exceptions;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.WorkflowLevels.Queries.GetWorkflowLevels
{
    public class GetWorkflowLevelsByActionQuery : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public WorkflowLevelAction WorkflowLevelAction { get; set; }
    }
    public class GetWorkflowLevelsByActionQueryHandler : IRequestHandler<GetWorkflowLevelsByActionQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetWorkflowLevelsByActionQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetWorkflowLevelsByActionQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                var roles = await _authService.GetRolesAsync(request.AccessToken, request.OrganisationId);

                var list = await _context.WorkflowLevels.Where(a => a.OrganisationId == request.OrganisationId).ToListAsync();
                list = list.Where(a => request.WorkflowLevelAction.ToString().IsIn(a.WorkflowLevelAction)).ToList();

                if (list == null)
                {
                    throw new NotFoundException(nameof(WorkflowLevel), request.OrganisationId);
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
                return Result.Failure($"Error retrieving Workflow levels by action. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }

}
