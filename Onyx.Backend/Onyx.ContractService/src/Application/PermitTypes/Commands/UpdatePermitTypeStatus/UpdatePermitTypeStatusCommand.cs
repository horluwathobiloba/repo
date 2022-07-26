﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.PermitTypes.Queries.GetPermitTypes;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.PermitTypes.Commands.UpdatePermitTypeStatus
{
    public class UpdatePermitTypeStatusCommand : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public int Id { get; set; }
        public Status Status { get; set; }
        public string UserId { get; set; }
    }

    public class UpdatePermitTypeStatusCommandHandler : IRequestHandler<UpdatePermitTypeStatusCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public UpdatePermitTypeStatusCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(UpdatePermitTypeStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);

                var entity = await _context.PermitTypes.FirstOrDefaultAsync(x => x.OrganisationId == request.OrganisationId && x.Id == request.Id);
                if (entity == null)
                {
                    return Result.Failure("Invalid license type!");
                }

                string message = "";
                switch (request.Status)
                {
                    case Status.Inactive:
                        entity.Status = Domain.Enums.Status.Inactive;
                        message = "Permit type is now inactive!";
                        break;
                    case Status.Active:
                        entity.Status = Domain.Enums.Status.Active;
                        message = "Permit type was successfully activated!";
                        break;
                    case Status.Deactivated:
                        message = "Permit type activation was deactivated!";
                        break;
                    default:
                        break;
                }

                entity.Status = request.Status;
                entity.StatusDesc = request.Status.ToString();
                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<PermitTypeDto>(entity);
                return Result.Success(message, result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Permit type status update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }
    }
}
