using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.WorkflowLevels.Queries.GetWorkflowLevels
{
    public class GetWorkflowLevelSummary : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
    }


    public class GetContractSummaryByIdQueryHandler : IRequestHandler<GetWorkflowLevelSummary, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        public GetContractSummaryByIdQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetWorkflowLevelSummary request, CancellationToken cancellationToken)
        {
            if (request.OrganisationId <= 0)
            {
                return Result.Failure($"Organisation must be specified.");
            }
            var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);

            // To do: refactor this code
            var approvedContract = await _context.WorkflowLevels.Where(x => x.OrganisationId == request.OrganisationId).CountAsync();
            var contract = await _context.WorkflowLevels.Where(x => x.OrganisationId == request.OrganisationId).ToListAsync();
            var totalContract = contract.Count;
            var inProgressContract = await _context.WorkflowLevels.Where(x => x.OrganisationId == request.OrganisationId).CountAsync();
            var expiredContract = await _context.WorkflowLevels.Where(x => x.OrganisationId == request.OrganisationId).CountAsync();
            var pendingContract = await _context.WorkflowLevels.Where(x => x.OrganisationId == request.OrganisationId).CountAsync();
            var disApprovedContract = await _context.WorkflowLevels.Where(x => x.OrganisationId == request.OrganisationId).CountAsync();

            return Result.Success(
                new
                {
                    totalContract = totalContract,
                    inProgressContract = inProgressContract,
                    expiredContract = expiredContract,
                    pendingContract = pendingContract,
                    approvedContract = approvedContract,
                    disApprovedContract = disApprovedContract
                });

        }
    }
}
