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
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.WorkflowPhases.Commands.CreateWorkflowPhase
{
    public class CreateWorkflowPhaseCommand : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public int ContractTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public WorkflowSequence WorkflowSequence { get; set; }
        public WorkflowUserCategory WorkflowUserCategory { get; set; }
        public string UserId { get; set; }
    }


    public class CreateWorkflowPhaseCommandHandler : IRequestHandler<CreateWorkflowPhaseCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreateWorkflowPhaseCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(CreateWorkflowPhaseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);

                var exists = await _context.WorkflowPhases.AnyAsync(x => x.OrganisationId == request.OrganisationId && x.ContractTypeId == request.ContractTypeId
                && x.Name.ToLower() == request.Name.ToLower());

                if (exists)
                {
                    return Result.Failure($"Workflow phase name already exists!");
                }

                var entity = new WorkflowPhase
                {
                    ContractTypeId = request.ContractTypeId,
                    Description = request.Description,
                    OrganisationId = request.OrganisationId,
                    Name = request.Name,
                    WorkflowSequence = request.WorkflowSequence.ToString(),
                    WorkflowUserCategory = request.WorkflowUserCategory.ToString(),

                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = request.UserId,
                    LastModifiedDate = DateTime.Now,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString()
                };

                await _context.WorkflowPhases.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<WorkflowPhaseDto>(entity);
                return Result.Success("Workflow phase was created successfully", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Workflow phase creation was not successful. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message} ");
            }
        }
    }

}
