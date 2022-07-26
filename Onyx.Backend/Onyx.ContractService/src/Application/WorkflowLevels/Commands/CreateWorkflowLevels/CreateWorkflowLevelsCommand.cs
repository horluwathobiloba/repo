using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.WorkflowLevels.Queries.GetWorkflowLevels;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.WorkflowLevels.Commands.CreateWorkflowLevels
{
    public class CreateWorkflowLevelsCommand : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public int WorkflowPhaseId { get; set; }
        public List<CreateWorkflowLevelRequest> WorkflowLevels { get; set; }
        public string UserId { get; set; }
    }

    public class CreateWorkflowLevelsCommandHandler : IRequestHandler<CreateWorkflowLevelsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreateWorkflowLevelsCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(CreateWorkflowLevelsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                var roles = await _authService.GetRolesAsync(request.AccessToken,  request.OrganisationId);

                var list = new List<WorkflowLevel>();
                await _context.BeginTransactionAsync();

                foreach (var item in request.WorkflowLevels)
                {
                    if (!roles.Entity.Any(r => r.Id == item.RoleId))
                    {
                        throw new Exception("Invalid role specified in the request!");
                    }

                    var exists = await _context.WorkflowLevels
                        .AnyAsync(x => x.OrganisationId == request.OrganisationId && x.WorkflowPhaseId == request.WorkflowPhaseId
                        && x.RoleId == item.RoleId && x.WorkflowLevelAction == item.WorkflowLevelAction.ToString());

                    if (exists)
                    {
                        throw new Exception($"Workflow level already exists for {item.RoleId} !");
                    }
                    var entity = new WorkflowLevel
                    {
                        WorkflowPhaseId = request.WorkflowPhaseId,
                        RoleId = item.RoleId,
                        Rank = item.Rank,
                        WorkflowLevelAction = item.WorkflowLevelAction.ToString(),

                        OrganisationId = request.OrganisationId,
                        OrganisationName = _authService.Organisation.OrganisationName,
                        CreatedBy = request.UserId,
                        CreatedDate = DateTime.Now,
                        LastModifiedBy = request.UserId,
                        LastModifiedDate = DateTime.Now,
                        Status = Status.Active,
                        StatusDesc = Status.Active.ToString()
                    };
                    list.Add(entity);
                }
                await _context.WorkflowLevels.AddRangeAsync(list);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                var result = _mapper.Map<List<WorkflowLevelDto>>(list);
                return Result.Success("Workflow levels created successfully!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Workflow levels creation failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
