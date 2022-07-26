using Onyx.WorkFlowService.Application.Common.Exceptions;
using Onyx.WorkFlowService.Application.Common.Interfaces;
using Onyx.WorkFlowService.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Onyx.WorkFlowService.Application.Common.Models;
using System;

namespace Onyx.WorkFlowService.Application.Organizations.Commands.ChangeOrganizationStatus
{
    public class ChangeOrganizationStatusCommand : IRequest<Result>
    {
        public int OrganizationId { get; set; }
        public string UserId { get; set; }
    }

    public class ChangeOrganizationStatusCommandHandler : IRequestHandler<ChangeOrganizationStatusCommand,Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public ChangeOrganizationStatusCommandHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<Result> Handle(ChangeOrganizationStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string message = "";
                var user = await _identityService.GetUserById(request.UserId);
                if (user.staff == null)
                {
                    return Result.Failure(new string[] { "Invalid Staff" });
                }
                var entity = await _context.Organizations.FindAsync(request.OrganizationId);
                if (entity == null)
                {
                    return Result.Failure(new string[] {"Invalid User"});
                }
                switch (entity.Status)
                {
                    case Domain.Enums.Status.Active:
                        entity.Status = Domain.Enums.Status.Inactive;
                        message = "Organization deactivation was successful";
                        break;
                    case Domain.Enums.Status.Inactive:
                        entity.Status = Domain.Enums.Status.Active;
                        message = "Organization activation was successful";
                        break;
                    case Domain.Enums.Status.Deactivated:
                        entity.Status = Domain.Enums.Status.Active;
                        message = "Organization activation was successful";
                        break;
                    default:
                        break;
                }
                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success(message);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Organization status change was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
