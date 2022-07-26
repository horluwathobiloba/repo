using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.WorkflowPhases.Queries.GetWorkflowPhases;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.WorkflowPhases.Commands.ChangeWorkflowPhaseStatus
{
    public class UpdateWorkflowPhaseStatusCommand : AuthToken, IRequest<Result>
    {

        public int OrganisationId { get; set; }
        public int Id { get; set; }
        public int ContractTypeId { get; set; }
        public Status Status { get; set; }
        public string UserId { get; set; }
    }

    public class ChangeWorkflowPhaseStatusCommandHandler : IRequestHandler<UpdateWorkflowPhaseStatusCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;


        public ChangeWorkflowPhaseStatusCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(UpdateWorkflowPhaseStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);

                var entity = await _context.WorkflowPhases.FirstOrDefaultAsync(x => x.Id == request.Id);
                if (entity == null)
                {
                    return Result.Failure("Invalid Workflow phase!");
                }

                string message = "";
                switch (request.Status)
                {
                    case Status.Inactive:
                        entity.Status = Domain.Enums.Status.Inactive;
                        message = "Workflow phase is now inactive!";
                        break;
                    case Status.Active:
                        entity.Status = Domain.Enums.Status.Active;
                        message = "Workflow phase was successfully activated!";
                        break;
                    case Status.Deactivated:
                        message = "Workflow phase activation was deactivated!";
                        break;
                    default:
                        break;
                }

                entity.Status = request.Status;
                entity.StatusDesc = request.Status.ToString();
                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<WorkflowPhaseDto>(entity);
                return Result.Success(message, result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Workflow phase status update failed { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }
    }
}
