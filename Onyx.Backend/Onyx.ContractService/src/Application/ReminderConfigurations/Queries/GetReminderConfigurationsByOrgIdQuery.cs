﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ReminderConfigurations.Queries
{
    public class GetReminderConfigurationsByOrgIdQuery : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
    }
    public class GetReminderConfigurationsByOrgIdQueryHandler : IRequestHandler<GetReminderConfigurationsByOrgIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;
        public GetReminderConfigurationsByOrgIdQueryHandler(IApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        public async Task<Result> Handle(GetReminderConfigurationsByOrgIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                var entities = await _context.ReminderConfigurations.Where(x => x.OrganisationId == request.OrganisationId).ToListAsync();
                if (entities == null || entities.Count==0)
                {
                    return Result.Failure($"Reminder Configuration with this {request.OrganisationId} does not exist.");
                }
                return Result.Success(entities);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Get reminder recipient by OrgId failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message} ");
            }
        }
    }
}
