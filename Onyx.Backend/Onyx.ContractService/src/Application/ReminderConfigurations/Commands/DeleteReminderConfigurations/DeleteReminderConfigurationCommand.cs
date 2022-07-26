using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ReminderConfigurations.Commands
{
    public class DeleteReminderConfigurationsCommand : AuthToken, IRequest<Result>
    {
        public int Id { get; set; }
        public int OrganisationId { get; set; }

        public class DeleteReminderConfigurationsCommandHandler : IRequestHandler<DeleteReminderConfigurationsCommand, Result>
        {
            private readonly IApplicationDbContext _context;
            private readonly IAuthService _authService;
            public DeleteReminderConfigurationsCommandHandler(IApplicationDbContext context, IAuthService authService)
            {
                _context = context;
                _authService = authService;
            }
            public async Task<Result> Handle(DeleteReminderConfigurationsCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);

                    var entities = await _context.ReminderConfigurations.FirstOrDefaultAsync(x => x.OrganisationId == request.OrganisationId && x.Id == request.Id);
                    if (entities == null)
                    {
                        return Result.Failure($"Reminder recipient with this Id {request.Id} does not exist.");
                    }
                    _context.ReminderConfigurations.Remove(entities);
                    await _context.SaveChangesAsync(cancellationToken);

                    return Result.Success("Reminder recipient is now deleted!", entities);
                }
                catch (Exception ex)
                {
                    return Result.Failure($"Reminder recipient status delete failed { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
                }
            }
        }
    }
}
