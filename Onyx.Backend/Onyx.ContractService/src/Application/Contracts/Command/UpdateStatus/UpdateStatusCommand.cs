using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.Contractaudit.Commands.CreateContractaudit;
using Onyx.ContractService.Application.Contracts.Queries.GetContracts;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.Contracts.Commands.UpdateStatus
{
    public class UpdateStatusCommand : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public int Id { get; set; }
        public Status Status { get; set; }
        public string UserId { get; set; }
    }

    public class UpdateStatusCommandHandler : IRequestHandler<UpdateStatusCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public UpdateStatusCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(UpdateStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                //get user object
                var user = await _authService.GetUserAsync(request.AccessToken, request.UserId);
                if (user==null)
                {
                    return Result.Failure("UserId is not valid");
                }
                var entity = await _context.Contracts.FirstOrDefaultAsync(x => x.OrganisationId == request.OrganisationId && x.Id == request.Id);
                if (entity == null)
                {
                    return Result.Failure("Invalid contract!");
                }

                //to get the old values of the contract before updating.
                
                var oldValuesEntity = entity;

                string message = "";
                switch (request.Status)
                {
                    case Status.Inactive:
                        entity.Status = Status.Inactive;
                        message = "Contract is now inactive!";
                        break;
                    case Status.Active:
                        entity.Status = Status.Active;
                        message = "Contract was successfully activated!";
                        break;
                    case Status.Deactivated:
                        message = "Contract was deactivated!";
                        break;
                    default:
                        break;
                }

                entity.Status = request.Status;
                entity.StatusDesc = request.Status.ToString();
                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;

                _context.Contracts.UpdateRange(entity);
                await _context.SaveChangesAsync(cancellationToken);

                //create audit log
                if (entity.DocumentType == DocumentType.Contract)
                {
                    var newValuesEntity = entity;

                    //create audit log for contract request
                    var command = new CreateContractAuditLogCommand
                    {
                        OrganisationId = request.OrganisationId,
                        OrganisationName = entity.OrganisationName,
                        LastModifiedBy = request.UserId,
                        RoleId = entity.RoleId,
                        RoleName = entity.RoleName,
                        FirstName = user.Entity.FirstName,
                        UserId = request.UserId,
                        LastName = user.Entity.LastName,
                        JobFunctionId = user.Entity.JobFunctionId,
                        JobFunctionName = user.Entity.JobFunction?.Name,
                        Module = Module.Contract.ToString(),
                        OldValue = oldValuesEntity,
                        NewValue = newValuesEntity,
                        Action = AuditType.Update.ToString(),
                    };
                    var handler = new CreateContractAuditLogCommandHandler(_context, _mapper);
                    var createAuditLog = await handler.Handle(command, cancellationToken);
                }

                if (entity.DocumentType == DocumentType.Permit)
                {
                    var newValuesEntity = entity;

                    //create audit log for contract request
                    var command = new CreateContractAuditLogCommand
                    {
                        OrganisationId = request.OrganisationId,
                        OrganisationName = entity.OrganisationName,
                        RoleId = entity.RoleId,
                        RoleName = entity.RoleName,
                        FirstName = user.Entity.FirstName,
                        UserId = request.UserId,
                        LastName = user.Entity.LastName,
                        JobFunctionId = user.Entity.JobFunctionId,
                        JobFunctionName = user.Entity.JobFunction?.Name,
                        Module = Module.Permit.ToString(),
                        OldValue = oldValuesEntity,
                        NewValue = newValuesEntity,
                        Action = AuditType.Update.ToString(),
                    };
                    var handler = new CreateContractAuditLogCommandHandler(_context, _mapper);
                    var createAuditLog = await handler.Handle(command, cancellationToken);
                }

                var result = _mapper.Map<ContractDto>(entity);
                return Result.Success(message, entity);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Contract status update failed { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }
    }
}
