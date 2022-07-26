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

namespace Onyx.ContractService.Application.ReminderRecipients.Queries
{
    public class GetAllReminderRecipientsQuery : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
    }
    public class GetAllReminderRecipientsHandler : IRequestHandler<GetAllReminderRecipientsQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;
        public GetAllReminderRecipientsHandler(IApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        public async Task<Result> Handle(GetAllReminderRecipientsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);

                var entities = await _context.ReminderRecipients.Where(a => a.OrganisationId == request.OrganisationId).Include(x => x.Contract).ToListAsync();
                if (entities == null)
                {
                    return Result.Failure("There is no reminder recipient found for this client");
                }
                return Result.Success($"{entities.Count}(s) found.", entities);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Get all reminder recipients failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message} ");
            }
        }
    }
}
