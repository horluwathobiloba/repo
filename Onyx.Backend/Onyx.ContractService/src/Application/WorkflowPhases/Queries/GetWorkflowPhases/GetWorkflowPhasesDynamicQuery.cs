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

namespace Onyx.ContractService.Application.WorkflowLevels.Queries.GetWorkflowLevels
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
                if (request.OrganisationId <= 0)
                {
                    return Result.Failure($"Organisation must be specified.");
                }
                if (string.IsNullOrEmpty(request.SearchText))
                {
                    return Result.Failure($"Search text must be specified.");
                }
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);

                var list = await _context.WorkflowLevels
                    .Where(a => a.OrganisationId == request.OrganisationId).ToListAsync();
                list = list.Where(a => request.SearchText.IsIn(a.Name)
                    || request.SearchText.IsIn(a.Description)
                    || request.SearchText.IsIn(a.WorkflowLevelAction)
                    || request.SearchText.IsIn(a.Rank.ToString())
                    || request.SearchText.IsIn(a.WorkflowPhase.Name)
                    || request.SearchText.IsIn(a.WorkflowPhase.ContractType.Name)).ToList();

                if (list == null)
                {
                    throw new NotFoundException(nameof(WorkflowLevel));
                }
                var result = _mapper.Map<List<WorkflowLevelDto>>(list);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving Workflow levels. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }

}
