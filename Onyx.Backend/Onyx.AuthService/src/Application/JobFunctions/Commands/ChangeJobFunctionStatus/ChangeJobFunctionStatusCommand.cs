using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.AuthService.Application.Common.Interfaces;
using Onyx.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.AuthService.Application.JobFunctions.Commands.ChangeJobFunctionStatus
{
    public class ChangeJobFunctionStatusCommand:IRequest<Result>
    {
        public int OrganizationId { get; set; }
        public string UserId { get; set; }
        public int JobFunctionId { get; set; }
    }

    public class ChangeJobFunctionStatusCommandHandler : IRequestHandler<ChangeJobFunctionStatusCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        public ChangeJobFunctionStatusCommandHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }
        public async Task<Result> Handle(ChangeJobFunctionStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _identityService.GetUserById(request.UserId);
                if (user.staff == null)
                {
                    return Result.Failure(new string[] { "Invalid User" });
                }
                var org = await _context.Organizations.FirstOrDefaultAsync(a => a.Id == user.staff.OrganizationId);
                if (org == null)
                {
                    return Result.Failure(new string[] { "Unable to change jobFunction status. User does not belong to Organization" });
                }
                string message = "";
                var job = await _context.JobFunctions.FirstOrDefaultAsync(a => a.Id == request.JobFunctionId);
                if (job == null)
                {
                    return Result.Failure(new string[] { "Invalid job" });
                }
                switch (job.Status)
                {
                    case Domain.Enums.Status.Active:
                        job.Status = Domain.Enums.Status.Inactive;
                        message = "job deactivation was successful";
                        break;
                    case Domain.Enums.Status.Inactive:
                        job.Status = Domain.Enums.Status.Active;
                        message = "job activation was successful";
                        break;
                    case Domain.Enums.Status.Deactivated:
                        job.Status = Domain.Enums.Status.Active;
                        message = "job activation was successful";
                        break;
                    default:
                        break;
                }
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success(message);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "job status change was not successful", ex?.Message + ex?.InnerException.Message });
            }
        }
    }
}
