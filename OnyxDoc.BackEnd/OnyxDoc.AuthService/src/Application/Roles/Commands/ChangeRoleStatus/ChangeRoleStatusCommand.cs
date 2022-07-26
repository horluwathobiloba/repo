using OnyxDoc.AuthService.Application.Common.Exceptions;
using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using OnyxDoc.AuthService.Application.Common.Models;
using System;
using Microsoft.EntityFrameworkCore;

namespace OnyxDoc.AuthService.Application.Roles.Commands.ChangeRole
{
    public class ChangeRoleStatusCommand :  IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string UserId { get; set; }
        public int RoleId { get; set; }
    }

    public class ChangeRoleCommandHandler : IRequestHandler<ChangeRoleStatusCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public ChangeRoleCommandHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<Result> Handle(ChangeRoleStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _identityService.GetUserById(request.UserId);
                if (user.user == null)
                {
                    return Result.Failure(new string[] { "Invalid User" });
                }
                var org = await _context.Subscribers.FirstOrDefaultAsync(a => a.Id == user.user.SubscriberId);
                if (org == null)
                {
                    return Result.Failure(new string[] { "Unable to change role status. User does not belong to Subscriber" });
                }
                string message = "";
                var role = await _context.Roles.FirstOrDefaultAsync(a=>a.Id == request.RoleId);
                if (role == null)
                {
                    return Result.Failure(new string[] { "Invalid Role" });
                }
                switch (role.Status)
                {
                    case Domain.Enums.Status.Active:
                        role.Status = Domain.Enums.Status.Inactive;
                        message = "Role deactivation was successful";
                        break;
                    case Domain.Enums.Status.Inactive:
                        role.Status = Domain.Enums.Status.Active;
                        message = "Role activation was successful";
                        break;
                    case Domain.Enums.Status.Deactivated:
                        role.Status = Domain.Enums.Status.Active;
                        message = "Role activation was successful";
                        break;
                    default:
                        break;
                }
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success(message);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Role status change was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
