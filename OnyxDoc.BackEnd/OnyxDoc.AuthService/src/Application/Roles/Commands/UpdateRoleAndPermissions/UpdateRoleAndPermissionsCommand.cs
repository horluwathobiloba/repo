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
using OnyxDoc.AuthService.Domain.ViewModels;

namespace OnyxDoc.AuthService.Application.RolePermissions.Commands.UpdateRoleAndPermissions
{
    public partial class UpdateRoleAndPermissionsCommand :  IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string UserId { get; set; }
        public int RoleId { get; set; }
        public RoleAccessLevel RoleAccessLevel { get; set; }
        public string Name { get; set; }
        public Dictionary<string, List<RolePermissionsVm>> RolePermissions { get; set; }
        public int CheckedCount { get; set; }

    }

    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleAndPermissionsCommand,Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly ISqlService _sqlService;


        public UpdateRoleCommandHandler(IApplicationDbContext context, IIdentityService identityService, ISqlService sqlService)
        {
            _context = context;
            _identityService = identityService;
            _sqlService = sqlService;
        }

        public async Task<Result> Handle(UpdateRoleAndPermissionsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _identityService.GetUserByIdAndSubscriber(request.UserId, request.SubscriberId);
                if (user.user == null)
                {
                    return Result.Failure(new string[] { "Unable to update role.Invalid User ID and Subscriber credentials!" });
                }
                var getRoleForUpdate = await _context.Roles.FirstOrDefaultAsync(a => a.Id == request.RoleId);
                if (getRoleForUpdate == null)
                {
                    return Result.Failure(new string[] { "Invalid Role" });
                }
                getRoleForUpdate.Name = request.Name.Trim();
                getRoleForUpdate.LastModifiedById = request.UserId;
                getRoleForUpdate.LastModifiedDate = DateTime.Now;
                getRoleForUpdate.RoleAccessLevel = request.RoleAccessLevel;
                getRoleForUpdate.RoleAccessLevelDesc = request.RoleAccessLevel.ToString();
                getRoleForUpdate.SubscriberId = request.SubscriberId;
                getRoleForUpdate.CheckedCount = request.CheckedCount;
                _context.Roles.Update(getRoleForUpdate);

                //Role Permissions
                var getRolePermissions = await _context.RolePermissions.Where(a => a.RoleId == request.RoleId).ToListAsync();
                if (getRolePermissions == null)
                {
                    return Result.Failure(new string[] { "Invalid Role Permissions for update" });
                }
                if (request.RolePermissions == null || request.RolePermissions.Count == 0)
                {
                    return Result.Failure(new string[] { "No Role Permissions for update", });
                }
                foreach (var  permission in request.RolePermissions)
                {
                    foreach (var item in permission.Value)
                    {
                        if (getRolePermissions.Any(a => a.Permission == item.Permission.ToString() &&
                                   a.Status == item.Status))
                        {
                            continue;
                        }
                        var permissionForUpdate = getRolePermissions.FirstOrDefault(a => a.Permission == item.Permission && a.Status != item.Status);
                        if (permissionForUpdate != null)
                        {
                            permissionForUpdate.Status = item.Status;
                            _context.RolePermissions.Update(permissionForUpdate);
                        } 
                    }
                }
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                return Result.Success("Role and Permissions created successfully");
            }
            catch (Exception ex)
            {
                _context.RollbackTransaction();
                return Result.Failure(new string[] { "Role and Permissions creation was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
