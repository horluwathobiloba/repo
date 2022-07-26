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

namespace Onyx.ContractService.Application.ReminderConfigurations.Queries
{
    public class GetReminderConfigurationsByIdQuery : AuthToken, IRequest<Result>
    {
        public int Id { get; set; }
        public int OrganisationId { get; set; }
    }
    public class GetReminderConfigurationsByIdQueryHandler : IRequestHandler<GetReminderConfigurationsByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;
        public GetReminderConfigurationsByIdQueryHandler(IApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        public async Task<Result> Handle(GetReminderConfigurationsByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                var reminderConfig = await _context.ReminderConfigurations.Where(x => x.OrganisationId == request.OrganisationId & x.Id == request.Id).FirstOrDefaultAsync();
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
