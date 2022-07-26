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
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.WorkflowLevels.Commands.UpdateWorkflowLevels
{
    public class UpdateWorkflowLevelsCommand : AuthToken, IRequest<Result>
    {
        public List<UpdateWorkflowLevelRequest> WorkflowLevels { get; set; }
        public int OrganisationId { get; set; }
        public int WorkflowPhaseId { get; set; }
        public string UserId { get; set; }
    }

    public class UpdateWorkflowLevelsCommandHandler : IRequestHandler<UpdateWorkflowLevelsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public UpdateWorkflowLevelsCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;

        }
        public async Task<Result> Handle(UpdateWorkflowLevelsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                var roles = await _authService.GetRolesAsync(request.AccessToken, request.OrganisationId);

                var list = new List<WorkflowLevel>();

                await _context.BeginTransactionAsync();

                foreach (var item in request.WorkflowLevels)
                {
                    if(!roles.Entity.Any(r => r.Id == item.RoleId))
                    {
                        throw new Exception("Invalid role specified in the request!");
                    }

                    var entity = await _context.WorkflowLevels
                        .Where(x => x.OrganisationId == request.OrganisationId && x.Id == item.Id)
                        .Include(a => a.WorkflowPhase)
                        .ThenInclude(b => b.ContractType)
                        .FirstOrDefaultAsync();

                    if (entity == null)
                    {
                        //return Result.Failure($"Invalid workflow level specified.");
                        entity = new WorkflowLevel
                        {
                            OrganisationId = request.OrganisationId,
                            OrganisationName = _authService.Organisation.OrganisationName,
                            WorkflowPhaseId = request.WorkflowPhaseId,
                            RoleId = item.RoleId,
                            Rank = item.Rank,
                            WorkflowLevelAction = item.WorkflowLevelAction.ToString(),

                            CreatedBy = request.UserId,
                            CreatedDate = DateTime.Now,
                            LastModifiedBy = request.UserId,
                            LastModifiedDate = DateTime.Now,
                            Status = Status.Active,
                            StatusDesc = Status.Active.ToString()
                        };
                    }
                    else
                    {
                        var validationResult = await ValidateRequest(request, entity);

                        if (validationResult.Succeeded == false)
                        {
                            return validationResult;
                        }

                        // TO DO: check if there are no contract type workflow transctions in flight with this workflow action.
                        // Those workflow transctions must first be executed before the changes are made.
                        entity.WorkflowLevelAction = item.WorkflowLevelAction.ToString();
                        entity.LastModifiedBy = request.UserId;
                        entity.LastModifiedDate = DateTime.Now;
                    }
                    list.Add(entity);
                }

                _context.WorkflowLevels.UpdateRange(list);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                var result = _mapper.Map<List<WorkflowLevelDto>>(list);
                return Result.Success("workflow levels update was successful!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"WorkflowLevel update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }

        private async Task<Result> ValidateRequest(UpdateWorkflowLevelsCommand request, WorkflowLevel entity)
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
                var workflowLevelExists = false;

                if (entity.WorkflowPhase.WorkflowSequence.EqualTo(WorkflowSequence.Sequential.ToString()))
                {
                    //include rank in the search criteria for this workflow phase with sequential workflow progress
                    workflowLevelExists = await _context.WorkflowLevels
                        .AnyAsync(x => x.OrganisationId == request.OrganisationId && x.Id != entity.Id
                        && x.WorkflowPhaseId == entity.WorkflowPhaseId && x.RoleId == entity.RoleId && x.Rank == entity.Rank
                        && x.WorkflowLevelAction == entity.WorkflowLevelAction.ToString());
                }
                else if (entity.WorkflowPhase.WorkflowSequence.EqualTo(WorkflowSequence.NonSequential.ToString()))
                {
                    //Do not include rank in the search criteria for workflow phase with non-sequential workflow progress
                    workflowLevelExists = await _context.WorkflowLevels
                        .AnyAsync(x => x.OrganisationId == request.OrganisationId && x.Id != entity.Id && x.WorkflowPhaseId == entity.WorkflowPhaseId
                        && x.RoleId == entity.RoleId && x.WorkflowLevelAction == entity.WorkflowLevelAction.ToString());
                }
                else
                {
                    //do nothing
                }

                if (workflowLevelExists)
                {
                    return Result.Failure($"A Workflow level with the same configuration already exists in this workflow phase. Please change this configuration or modify the other Workflow level configuration before modifying this Workflow level.");
                }


                var workflowLevelsWithThisAction = await _context.WorkflowLevels.Where(x => x.OrganisationId == request.OrganisationId
                 && x.WorkflowPhase.ContractType.Id == contractType.Id && x.WorkflowLevelAction.Equals(entity.WorkflowLevelAction.ToString(), StringComparison.InvariantCultureIgnoreCase)).ToListAsync();

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
