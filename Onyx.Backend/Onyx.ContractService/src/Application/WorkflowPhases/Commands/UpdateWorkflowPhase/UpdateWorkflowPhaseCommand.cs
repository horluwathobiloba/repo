using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.WorkflowPhases.Queries.GetWorkflowPhases;
using Onyx.ContractService.Domain.Enums;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.WorkflowPhases.Commands.UpdateWorkflowPhase
{
    public class UpdateWorkflowPhaseCommand : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public int ContractTypeId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public WorkflowSequence WorkflowSequence { get; set; }
        public WorkflowUserCategory WorkflowUserCategory { get; set; }
        public string UserId { get; set; }
    }

    public class UpdateWorkflowPhaseCommandHandler : IRequestHandler<UpdateWorkflowPhaseCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;       
        private readonly IAuthService _authService;

        public UpdateWorkflowPhaseCommandHandler(IApplicationDbContext context, IMapper mapper ,IAuthService authService)
        {
            _context = context;
            _mapper = mapper; 
            _authService = authService;

        }
        public async Task<Result> Handle(UpdateWorkflowPhaseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId );
                //check if the new/existing name in the update request is not already configured for other workflow phases in this contract type
                var UpdatedNameExists = await _context.WorkflowPhases.AnyAsync(x => x.OrganisationId == request.OrganisationId
                && x.Id != request.Id
                && x.ContractTypeId == request.ContractTypeId
                && x.Name.EqualTo(request.Name));

                if (UpdatedNameExists)
                {
                    return Result.Failure($"A record with this product service type name {request.Name} already exists. Please change the name.");
                }

                var entity = await _context.WorkflowPhases.FirstOrDefaultAsync(x => x.OrganisationId == request.OrganisationId && x.Id == request.Id);

                if (entity == null)
                {
                    return Result.Failure($"Invalid Workflow phase specified.");
                }

                entity.ContractTypeId = request.ContractTypeId;
                entity.Name = request.Name;
                entity.Description = request.Description;
                entity.WorkflowSequence = request.WorkflowSequence.ToString();

                //TO DO: check if any workflow level for this phase exists. If none, then allow the user to edit the workflow user category
                if (_context.WorkflowPhases.Any(x => x.OrganisationId == request.OrganisationId && x.Id == request.Id))
                {
                    entity.WorkflowUserCategory = request.WorkflowUserCategory.ToString();  //Should a user be able to change the workflow user category? This should only be possible if there is no workflow level added to the phase yet. This is because the workflow levels will have to be reconfigured.
                }
                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;

                _context.WorkflowPhases.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<WorkflowPhaseDto>(entity);
                return Result.Success("Workflow phase was updated successfully", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Workflow phase update failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }


    }

}
