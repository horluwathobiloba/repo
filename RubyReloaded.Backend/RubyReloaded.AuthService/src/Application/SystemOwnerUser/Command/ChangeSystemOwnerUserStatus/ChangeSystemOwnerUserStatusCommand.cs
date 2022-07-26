using MediatR;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.SuperAdmin.Command.ChangeSuperAdmiUserStatus
{
    public class ChangeSystemOwnerUserStatusCommand:IRequest<Result>
    {
        public string UserId { get; set; }
        public string LoggedInUserId { get; set; }
    }
    public class ChangeSuperAdmiUserStatusCommandHnadler : IRequestHandler<ChangeSystemOwnerUserStatusCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        public ChangeSuperAdmiUserStatusCommandHnadler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<Result> Handle(ChangeSystemOwnerUserStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _identityService.GetUserById(request.UserId);
                if (user.user == null)
                {
                    return Result.Failure(new string[] { "Unable to change status.Invalid User ID and Organization credentials!" });
                }



                var result = await _identityService.ChangeUserStatusAsync(user.user);
                await _context.SaveChangesAsync(cancellationToken);
                if (result.Succeeded)
                    return Result.Success(result.Messages[0]);
                else
                    return Result.Failure(result.Messages);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "User status change was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
