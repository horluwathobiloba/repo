using Onyx.WorkFlowService.Application.Common.Exceptions;
using Onyx.WorkFlowService.Application.Common.Interfaces;
using Onyx.WorkFlowService.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Onyx.WorkFlowService.Application.Common.Models;
using System;

namespace Onyx.WorkFlowService.Application.Departments.Commands
{
    public class ChangeDepartmentStatusCommand : IRequest<Result>
    {
        public int OrganizationId { get; set; }
        public string UserId { get; set; }
        public int DepartmentId { get; set; }
       
    }

    public class ChangeDepartmentStatusCommandHandler : IRequestHandler<ChangeDepartmentStatusCommand,Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public ChangeDepartmentStatusCommandHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<Result> Handle(ChangeDepartmentStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string message = "";
                //check userid and organization
                var user = await _identityService.GetUserByIdAndOrganization(request.UserId, request.OrganizationId);
                if (user.staff == null)
                {
                    return Result.Failure(new string[] { "Unable to change status.Invalid User ID and Organization credentials!" });
                }
               
                var entity = await _context.Departments.FindAsync(request.DepartmentId);
                if (entity == null)
                {
                    return Result.Failure(new string[] {"Invalid Department"});
                }
                switch (entity.Status)
                {
                    case Domain.Enums.Status.Active:
                        entity.Status = Domain.Enums.Status.Inactive;
                        message = "Department deactivation was successful";
                        break;
                    case Domain.Enums.Status.Inactive:
                        entity.Status = Domain.Enums.Status.Active;
                        message = "Department activation was successful";
                        break;
                    case Domain.Enums.Status.Deactivated:
                        entity.Status = Domain.Enums.Status.Active;
                        message = "Department activation was successful";
                        break;
                    default:
                        break;
                }
                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success(message);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Department status change was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
