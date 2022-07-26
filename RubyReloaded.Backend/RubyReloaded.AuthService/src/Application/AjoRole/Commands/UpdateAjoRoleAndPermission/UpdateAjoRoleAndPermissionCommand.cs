using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Application.RolePermissions.Queries.GetRolePermissions;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.AjoRole.Commands.UpdateAjoRoleAndPermission
{
    public class UpdateAjoRoleAndPermissionCommand:IRequest<Result>
    {
        public int AjoId { get; set; }
        public string UserId { get; set; }
        public int RoleId { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public string Name { get; set; }
        public Dictionary<string, List<RolePermissionsVm>> RolePermissions { get; set; }
    }
    public class UpdateAjoRoleAndPermissionCommandHandler : IRequestHandler<UpdateAjoRoleAndPermissionCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly ISqlService _sqlService;
        public UpdateAjoRoleAndPermissionCommandHandler(IApplicationDbContext context, IIdentityService identityService, ISqlService sqlService)
        {
            _context = context;
            _identityService = identityService;
            _sqlService = sqlService;
        }
        public async Task<Result> Handle(UpdateAjoRoleAndPermissionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _context.BeginTransactionAsync();
                var getRoleForUpdate = await _context.AjoRoles.FirstOrDefaultAsync(a => a.Id == request.RoleId);
                if (getRoleForUpdate == null)
                {
                    return Result.Failure(new string[] { "Invalid Role" });
                }
                getRoleForUpdate.Name = request.Name.Trim();
                getRoleForUpdate.LastModifiedById = request.UserId;
                getRoleForUpdate.LastModifiedDate = DateTime.Now;
                getRoleForUpdate.AccessLevel = request.AccessLevel;
                getRoleForUpdate.AccessLevelDesc = request.AccessLevel.ToString();
                getRoleForUpdate.AjoId = request.AjoId;

                _context.AjoRoles.Update(getRoleForUpdate);
                await _context.SaveChangesAsync(cancellationToken);
                var getRolePermissions = await _context.RolePermissions.Where(a => a.RoleId == request.RoleId).ToListAsync();
                if (getRolePermissions == null)
                {
                    return Result.Failure(new string[] { "Invalid Role Permissions for update" });
                }
                if (request.RolePermissions == null || request.RolePermissions.Count == 0)
                {
                    return Result.Failure(new string[] { "No Role Permissions for update", });
                }
                foreach (var permission in request.RolePermissions)
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
                return Result.Success();
            }
            catch (Exception ex)
            {
                _context.RollbackTransaction();
                return Result.Failure(new string[] { "Role and Permissions creation was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
