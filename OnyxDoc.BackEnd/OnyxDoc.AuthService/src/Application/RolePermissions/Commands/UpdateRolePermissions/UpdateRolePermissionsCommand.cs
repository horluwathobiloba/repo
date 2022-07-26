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
using System.Collections.Generic;
using System.Linq;

namespace OnyxDoc.AuthService.Application.RolePermissions.Commands.CreateRolePermissions
{
    public partial class UpdateRolePermissionsCommand :  IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string UserId { get; set; }
        public int RoleId { get; set; }
        public RoleAccessLevel RoleAccessLevel { get; set; }
        public List<string> RolePermissions { get; set; }

    }

    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRolePermissionsCommand,Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
    

        public UpdateRoleCommandHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<Result> Handle(UpdateRolePermissionsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _identityService.GetUserByIdAndSubscriber(request.UserId, request.SubscriberId);
                if (user.user == null)
                {
                    return Result.Failure(new string[] { "Unable to update role.Invalid User ID and Subscriber credentials!" });
                }
                var getRolePermissions = await _context.RolePermissions.Where(a=>a.RoleId == request.RoleId).ToListAsync();
                if (getRolePermissions == null)
                {
                    return Result.Failure(new string[] { "Invalid Role Permissions for update" });
                }
                if (request.RolePermissions == null || request.RolePermissions.Count == 0)
                {
                    return Result.Failure(new string[] { "Please input valid Role Permissions", });
                }
                 foreach (var item in request.RolePermissions)
                 {
                    var permission = item.Trim();
                    if (getRolePermissions.Any(a => a.Permission != permission))
                    {
                        var rolePermissions = new RolePermission
                        {

                            SubscriberId = request.SubscriberId,
                            RoleAccessLevel = request.RoleAccessLevel,
                            Permission = permission.Trim(),
                            RoleId = request.RoleId,
                            CreatedById = request.UserId,
                            CreatedDate = DateTime.Now,
                            Status = Status.Active
                        };
                        await _context.RolePermissions.AddRangeAsync(rolePermissions);
                        //await _context.RolePermissions.AddAsync(rolePermissions);
                    }
                  }
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Role Permissions updated successfully", getRolePermissions);
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Error updating role permissions: ", ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
