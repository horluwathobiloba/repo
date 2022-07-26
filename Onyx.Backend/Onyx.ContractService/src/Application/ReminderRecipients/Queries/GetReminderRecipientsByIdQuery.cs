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
    public class GetReminderRecipientsByIdQuery : AuthToken, IRequest<Result>
    {
        public int Id { get; set; }
        public int OrganisationId { get; set; }
    }
    public class GetReminderRecipientsByIdQueryHandler : IRequestHandler<GetReminderRecipientsByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;
        public GetReminderRecipientsByIdQueryHandler(IApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        public async Task<Result> Handle(GetReminderRecipientsByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                //check if it exist
                var list = await _context.ReminderRecipients.Where(x => x.OrganisationId == request.OrganisationId & x.Id == request.Id).FirstOrDefaultAsync();
                if (list == null)
                {
                    return Result.Failure($"Reminder Recipient with this contract does not exist.");
                }
                return Result.Success(list);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Get reminder recipient by id failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message} ");
            }
        }
    }
}
