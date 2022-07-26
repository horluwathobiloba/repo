using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.WorkflowPhases.Queries.GetWorkflowPhases;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.WorkflowPhases.Commands.CreateWorkflowPhaseAndLevels
{
    public class CreateWorkflowPhaseAndLevelsCommand : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public int ContractTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public WorkflowSequence WorkflowSequence { get; set; }
        public WorkflowUserCategory WorkflowUserCategory { get; set; }
        public List<CreateWorkflowLevelRequest> WorkflowLevelRequests { get; set; }
        public string UserId { get; set; }

    }

    public class CreateWorkflowPhaseAndLevelsCommandHandler : IRequestHandler<CreateWorkflowPhaseAndLevelsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreateWorkflowPhaseAndLevelsCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(CreateWorkflowPhaseAndLevelsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                var roles = await _authService.GetRolesAsync(request.AccessToken, request.OrganisationId);

                var exists = await _context.WorkflowPhases.AnyAsync(x => x.OrganisationId == request.OrganisationId
                && x.ContractTypeId == request.ContractTypeId && x.Name.Equals(request.Name, StringComparison.InvariantCultureIgnoreCase));

                if (exists)
                {
                    return Result.Failure($"Workflow phase name already exists!");
                }

                await _context.BeginTransactionAsync();

                var entity = new WorkflowPhase
                {
                    ContractTypeId = request.ContractTypeId,
                    Description = request.Description,
                    Name = request.Name,
                    WorkflowSequence = request.WorkflowSequence.ToString(),
                    WorkflowUserCategory = request.WorkflowUserCategory.ToString(),
                    OrganisationId = request.OrganisationId,
                    OrganisationName = request.OrganisationName,
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = request.UserId,
                    LastModifiedDate = DateTime.Now,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString()
                };

                await _context.WorkflowPhases.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);

                foreach (var item in request.WorkflowLevelRequests)
                {
                    if (!roles.Entity.Any(r => r.Id == item.RoleId))
                    {
                        throw new Exception("Invalid role specified in the request!");
                    }
                    entity.WorkflowLevels.Add(await this.Convert(item, entity));
                }

                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                var result = _mapper.Map<WorkflowPhaseDto>(entity);
                return Result.Success("Workflow phase was created successfully", entity);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Workflow phase creation was not successful. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message} ");
            }
        }

        private async Task<WorkflowLevel> Convert(CreateWorkflowLevelRequest item, WorkflowPhase workflowPhase)
        {
            var exists = await _context.WorkflowLevels.AnyAsync(x => x.OrganisationId == workflowPhase.OrganisationId
            && x.WorkflowPhaseId == workflowPhase.Id && x.Rank == item.Rank && x.RoleId == item.RoleId
                && x.WorkflowLevelAction.Equals(item.WorkflowLevelAction.ToString(), StringComparison.InvariantCultureIgnoreCase));

            if (exists)
            {
                throw new Exception($"Workflow level configuration already exists for {item.RoleId} !");
            }

            var entity = new WorkflowLevel
            {
                OrganisationId = workflowPhase.OrganisationId,
                OrganisationName = workflowPhase.OrganisationName,
                WorkflowPhaseId = workflowPhase.Id,
                RoleId = item.RoleId,
                RoleName = item.RoleName,
                Rank = item.Rank,
                WorkflowLevelAction = item.WorkflowLevelAction.ToString(),
                CreatedBy = workflowPhase.CreatedBy,
                CreatedDate = DateTime.Now,
                LastModifiedBy = workflowPhase.LastModifiedBy,
                LastModifiedDate = DateTime.Now,
                Status = Status.Active,
                StatusDesc = Status.Active.ToString()
            };

            return entity;
        }

    }
}
