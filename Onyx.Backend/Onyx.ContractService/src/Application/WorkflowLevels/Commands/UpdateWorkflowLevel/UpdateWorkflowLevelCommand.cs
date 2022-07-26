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
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.WorkflowLevels.Commands.UpdateWorkflowLevel
{
    public class UpdateWorkflowLevelCommand :AuthToken, IRequest<Result>
    {
        public int Id { get; set; }
        public int OrganisationId { get; set; }
        public int WorkflowPhaseId { get; set; }
        public int RoleId { get; set; }
        public int Rank { get; set; }
        public WorkflowLevelAction WorkflowLevelAction { get; set; }
        public string UserId { get; set; }
    }

    public class UpdateWorkflowLevelCommandHandler : IRequestHandler<UpdateWorkflowLevelCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;        
        private readonly IAuthService _authService;        

        public UpdateWorkflowLevelCommandHandler(IApplicationDbContext context, IMapper mapper,IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;            
        }
        public async Task<Result> Handle(UpdateWorkflowLevelCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId );
                var role = await _authService.ValidateRole(request.AccessToken,  request.RoleId);

                var entity = await _context.WorkflowLevels
                 .Where(x => x.OrganisationId == request.OrganisationId && x.Id == request.Id)
                 .Include(a => a.WorkflowPhase)
                 .ThenInclude(b => b.ContractType)
                 .FirstOrDefaultAsync();

                var validationResult = await ValidateRequest(request, entity);

                if (validationResult.Succeeded == false)
                {
                    return validationResult;
                }

                // TO DO: check if there are no contract type workflow transctions in flight with this workflow action.  Those workflow transctions must first be executed before the changes are made.
                entity.WorkflowLevelAction = request.WorkflowLevelAction.ToString();

                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;

                _context.WorkflowLevels.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<WorkflowLevelDto>(entity);
                return Result.Success("Workflow level was updated successfully", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Workflow level update failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        private async Task<Result> ValidateRequest(UpdateWorkflowLevelCommand request, WorkflowLevel entity)
        {
            try
            {
                if (entity == null)
                {
                    return Result.Failure($"Invalid workflow level specified.");
                }

                if (entity.WorkflowPhase == null)
                {
                    return Result.Failure($"Workflow level does not have a valid workflow phase. Please contact the administrator");
                }

                var contractType = entity.WorkflowPhase.ContractType;

                if (contractType == null)
                {
                    return Result.Failure($"Workflow level {entity.WorkflowPhase.Name} does not have a valid contract type. Please contact the administrator");
                }

                //check if the workflow level with the same RoleId and workflow level action already exists
                var WorkflowLevelConfigExists = false;

                if (entity.WorkflowPhase.WorkflowSequence.EqualTo(WorkflowSequence.Sequential.ToString()))
                {
                    //include rank in the search criteria for this workflow phase with sequential workflow progress
                    WorkflowLevelConfigExists = await _context.WorkflowLevels
                        .AnyAsync(x => x.OrganisationId == request.OrganisationId && x.Id != request.Id && x.WorkflowPhaseId == request.WorkflowPhaseId && x.RoleId == request.RoleId
                        && x.Rank == request.Rank && x.WorkflowLevelAction == request.WorkflowLevelAction.ToString());
                }
                else if (entity.WorkflowPhase.WorkflowSequence.EqualTo(WorkflowSequence.NonSequential.ToString()))
                {
                    //Do not include rank in the search criteria for workflow phase with non-sequential workflow progress
                    WorkflowLevelConfigExists = await _context.WorkflowLevels.AnyAsync(x => x.OrganisationId == request.OrganisationId
                    && x.Id != request.Id
                    && x.WorkflowPhaseId == request.WorkflowPhaseId
                    && x.RoleId == request.RoleId
                    && x.WorkflowLevelAction == request.WorkflowLevelAction.ToString());
                }
                else
                {
                    //do nothing
                }

                if (WorkflowLevelConfigExists)
                {
                    return Result.Failure($"A Workflow level with this same configuration already exists in this workflow phase. Please change this configuration or modify the other Workflow level configuration before modifying this Workflow level.");
                }

                entity.RoleId = request.RoleId;
                entity.Rank = request.Rank;

                var workflowLevelsWithThisAction = await _context.WorkflowLevels.Where(x => x.OrganisationId == request.OrganisationId
                 && x.WorkflowPhase.ContractType.Id == contractType.Id && x.WorkflowLevelAction.Equals(request.WorkflowLevelAction.ToString(), StringComparison.InvariantCultureIgnoreCase)).ToListAsync();

                //TO DO: check if the action being updated is a signatory action and confirm that the number of required signatories have not been exceeded for the contract type 
                switch (entity.WorkflowLevelAction.ParseEnum<WorkflowLevelAction>())
                {
                    case WorkflowLevelAction.ViewOnly:
                    case WorkflowLevelAction.Review:
                    case WorkflowLevelAction.Approve:
                        //do nothing
                        break;
                    case WorkflowLevelAction.InternalSignature:
                        if (contractType.EnableInternalSignatories == true && contractType.NumberOfInternalSignatories > 0 && workflowLevelsWithThisAction.Count() == contractType.NumberOfInternalSignatories)
                        {
                            return Result.Failure($"Number of required internal signatories required for the workflow configurations for contract type \"{contractType.Name}\"  has been reached. Please specify another action or increase the number of internal signatories required for the contract type that this workflow level is configured for.");
                        }
                        break;
                    case WorkflowLevelAction.ExternalSignature:
                        if (contractType.EnableExternalSignatories == true && contractType.NumberOfExternalSignatories > 0 && workflowLevelsWithThisAction.Count() == contractType.NumberOfExternalSignatories)
                        {
                            return Result.Failure($"Number of required external signatories required for the workflow configurations for contract type \"{contractType.Name}\"  has been reached. Please specify another action or increase the number of external signatories required for the contract type that this workflow level is configured for.");
                        }
                        break;
                    case WorkflowLevelAction.ThirdPartySignature:
                        if (contractType.EnableThirdPartySignatories == true && contractType.NumberOfThirdPartySignatories > 0 && workflowLevelsWithThisAction.Count() == contractType.NumberOfThirdPartySignatories)
                        {
                            return Result.Failure($"Number of required third party signatories required for the workflow configurations for contract type \"{contractType.Name}\"  has been reached. Please specify another action or increase the number of third party signatories required for the contract type that this workflow level is configured for.");
                        }
                        break;
                    case WorkflowLevelAction.WitnessSignature:
                        if (contractType.EnableWitnessSignatories == true && contractType.NumberOfWitnessSignatories > 0 && workflowLevelsWithThisAction.Count() == contractType.NumberOfWitnessSignatories)
                        {
                            return Result.Failure($"Number of required witness signatories required for the workflow configurations for contract type \"{contractType.Name}\"  has been reached. Please specify another action or increase the number of witness signatories required for the contract type that this workflow level is configured for.");
                        }
                        break;
                    default:
                        //do nothing
                        break;
                }

                return Result.Success("Validation was successful");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
