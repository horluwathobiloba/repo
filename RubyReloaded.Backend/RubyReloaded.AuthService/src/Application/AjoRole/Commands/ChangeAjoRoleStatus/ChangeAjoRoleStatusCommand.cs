using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.AjoRole.Commands.ChangeAjoRoleStatus
{
    public class ChangeAjoRoleStatusCommand:IRequest<Result>
    {
        public string LoggedInUserId { get; set; }
        public int RoleId { get; set; }
    }
    public class ChangeAjoRoleStatusCommandHandler : IRequestHandler<ChangeAjoRoleStatusCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        public ChangeAjoRoleStatusCommandHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<Result> Handle(ChangeAjoRoleStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _identityService.GetUserById(request.LoggedInUserId);
                if (user.user == null)
                {
                    return Result.Failure(new string[] { "Invalid User" });
                }
                //var org = await _context.Cooperatives.FirstOrDefaultAsync(a => a.Id == user.user.co);
                //if (org == null)
                //{
                //    return Result.Failure(new string[] { "Unable to change role status. User does not belong to Organization" });
                //}
                string message = "";
                var role = await _context.AjoRoles.FirstOrDefaultAsync(a => a.Id == request.RoleId);
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
