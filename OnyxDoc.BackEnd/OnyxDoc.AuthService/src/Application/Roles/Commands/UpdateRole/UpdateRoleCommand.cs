using OnyxDoc.AuthService.Application.Common.Exceptions;
using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;
using OnyxDoc.AuthService.Domain.Enums;
using OnyxDoc.AuthService.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace OnyxDoc.AuthService.Application.Roles.Commands.UpdateRole
{
    public partial class UpdateRoleCommand :  IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string UserId { get; set; }
        public int RoleId { get; set; }
        public string Name { get; set; }
        public RoleAccessLevel RoleAccessLevel { get; set; }
        public Status Status { get; set; }

    }

    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand,Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public UpdateRoleCommandHandler(IApplicationDbContext context,IIdentityService identityService)
        {
            _identityService = identityService;
            _context = context;
        }
        

        public async Task<Result> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _identityService.GetUserByIdAndSubscriber(request.UserId, request.SubscriberId);
                if (user.user == null)
                {
                    return Result.Failure(new string[] { "Unable to update role.Invalid User ID and Subscriber credentials!" });
                }
                var getRoleForUpdate = await _context.Roles.FirstOrDefaultAsync(a=>a.Id == request.RoleId);
                if (getRoleForUpdate == null)
                {
                    return Result.Failure(new string[] { "Invalid Role" });
                }
                getRoleForUpdate.Name = request.Name.Trim();
                getRoleForUpdate.LastModifiedById = request.UserId;
                getRoleForUpdate.LastModifiedDate = DateTime.Now;
                getRoleForUpdate.RoleAccessLevel = request.RoleAccessLevel;
                getRoleForUpdate.Status = request.Status;
                getRoleForUpdate.RoleAccessLevelDesc = request.RoleAccessLevel.ToString();
                getRoleForUpdate.SubscriberId = request.SubscriberId;
                getRoleForUpdate.StatusDesc = request.Status.ToString();

                _context.Roles.Update(getRoleForUpdate);
                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success("Role updated successfully", getRoleForUpdate);
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Error updating role: ", ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
