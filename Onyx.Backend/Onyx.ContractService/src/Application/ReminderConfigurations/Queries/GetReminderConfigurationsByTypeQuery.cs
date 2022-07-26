using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Onyx.ContractService.Domain.Enums;

namespace Onyx.ContractService.Application.ReminderConfigurations.Queries
{
    public class GetReminderConfigurationsByTypeQuery : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public ReminderType ReminderType { get; set; }
    }
    public class GetReminderConfigurationsByTypeQueryHandler : IRequestHandler<GetReminderConfigurationsByTypeQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;
        public GetReminderConfigurationsByTypeQueryHandler(IApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        public async Task<Result> Handle(GetReminderConfigurationsByTypeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                var reminderConfig = await _context.ReminderConfigurations.Where(x => x.OrganisationId == request.OrganisationId & x.ReminderType == request.ReminderType)
                                     .ToListAsync();
                var weeklyReminderConfig = await _context.WeeklyReminderSchedules.Where(a=>a.OrganisationId == request.OrganisationId).ToListAsync();
                var yearlyReminderConfig = await _context.YearlyReminderSchedules.Where(a => a.OrganisationId == request.OrganisationId).ToListAsync();
                foreach (var config in reminderConfig)
                {
                    config.WeeklyReminderSchedule = weeklyReminderConfig.Where(a => a.ReminderConfigurationId == config.Id).ToList();
                    config.YearlyReminderSchedule = yearlyReminderConfig.Where(a => a.ReminderConfigurationId == config.Id).FirstOrDefault();
                }
                if (reminderConfig == null)
                {
                    return Result.Failure($"Reminder Configurations does not exist.");
                }
                return Result.Success(reminderConfig);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Get reminder configurations by id failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message} ");
            }
        }
    }
}
