using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.WorkflowLevels.Queries.GetWorkflowLevels;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using ReventInject;
using ReventInject.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.WorkflowLevels.Commands.CreateWorkflowLevel
{
    public class CreateWorkflowLevelCommand : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public int WorkflowPhaseId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int Rank { get; set; }
        public WorkflowLevelAction WorkflowLevelAction { get; set; }
        public string UserId { get; set; }
    }


    public class CreateWorkflowLevelCommandHandler : IRequestHandler<CreateWorkflowLevelCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService; 

        public CreateWorkflowLevelCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(CreateWorkflowLevelCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                var role = await _authService.GetRoleAsync(request.AccessToken,  request.RoleId);

                var exists = await _context.WorkflowLevels
                    .AnyAsync(x => x.OrganisationId == request.OrganisationId && x.WorkflowPhaseId == request.WorkflowPhaseId
                    && x.RoleId == request.RoleId && x.WorkflowLevelAction == request.WorkflowLevelAction.ToString());

                if (exists)
                {
                    return Result.Failure($"Workflow level configuration already exists for this Role!");
                }
                var entity = new WorkflowLevel
                {
                    OrganisationId = request.OrganisationId,
                    WorkflowPhaseId = request.WorkflowPhaseId,
                    RoleId = request.RoleId,
                    Rank = request.Rank,
                    WorkflowLevelAction = request.WorkflowLevelAction.ToString(),
                    RoleName = request.RoleName,
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = request.UserId,
                    LastModifiedDate = DateTime.Now,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString()
                };

                await _context.WorkflowLevels.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<WorkflowLevelDto>(entity);
                return Result.Success("Workflow level was created successfully", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Workflow level creation was not successful. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message} ");
            }
        }
    }

}
