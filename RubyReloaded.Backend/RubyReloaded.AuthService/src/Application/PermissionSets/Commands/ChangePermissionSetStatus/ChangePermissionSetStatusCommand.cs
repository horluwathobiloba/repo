using RubyReloaded.AuthService.Application.Common.Exceptions;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using RubyReloaded.AuthService.Application.Common.Models;
using System;
using Microsoft.EntityFrameworkCore;

namespace RubyReloaded.AuthService.Application.PermissionSets.Commands.ChangePermissionSet
{
    public class ChangePermissionSetStatusCommand :  IRequest<Result>
    {
        public string UserId { get; set; }
        public int PermissionSetId { get; set; }
    }

    public class ChangePermissionSetCommandHandler : IRequestHandler<ChangePermissionSetStatusCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public ChangePermissionSetCommandHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<Result> Handle(ChangePermissionSetStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _identityService.GetUserById(request.UserId);
                if (user.user == null)
                {
                    return Result.Failure(new string[] { "Invalid User" });
                }
               
                string message = "";
                var feature = await _context.PermissionSets.FirstOrDefaultAsync(a=>a.Id == request.PermissionSetId);
                if (feature == null)
                {
                    return Result.Failure(new string[] { "Invalid PermissionSet" });
                }
                switch (feature.Status)
                {
                    case Domain.Enums.Status.Active:
                        feature.Status = Domain.Enums.Status.Inactive;
                        message = "PermissionSet deactivation was successful";
                        break;
                    case Domain.Enums.Status.Inactive:
                        feature.Status = Domain.Enums.Status.Active;
                        message = "PermissionSet activation was successful";
                        break;
                    case Domain.Enums.Status.Deactivated:
                        feature.Status = Domain.Enums.Status.Active;
                        message = "PermissionSet activation was successful";
                        break;
                    default:
                        break;
                }
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success(message);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "PermissionSet status change was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
