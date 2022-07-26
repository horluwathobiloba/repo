using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.Entities;
using RubyReloaded.AuthService.Domain.Enums;
using RubyReloaded.AuthService.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.Roles.Command.CreateRoleAndPermission
{
    public class CreateRoleAndPermissionCommand:IRequest<Result>
    {
        public int CooperativeId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public Dictionary<string, List<RolePermissionsVm>> RolePermissions { get; set; }
    }
    public class CreateRoleAndPermissionsCommandHandler : IRequestHandler<CreateRoleAndPermissionCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly ISqlService _sqlService;

        public CreateRoleAndPermissionsCommandHandler(IApplicationDbContext context, IIdentityService identityService, ISqlService sqlService)
        {
            _context = context;
            _identityService = identityService;
            _sqlService = sqlService;
        }

        public async Task<Result> Handle(CreateRoleAndPermissionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _identityService.GetUserById(request.UserId);
                if (user.user == null)
                {
                    return Result.Failure(new string[] { "Unable to create role.Invalid User ID and Organization credentials!" });
                }
                if (request.RolePermissions == null || request.RolePermissions.Count == 0)
                {
                    return Result.Failure(new string[] { "Please input valid role permissions", });
                }
                var cooperative = await _context.Cooperatives.FirstOrDefaultAsync(a => a.Id == request.CooperativeId);
                if (cooperative == null)
                {
                    return Result.Failure(new string[] { "Invalid organization details" });
                }
                var roledetails = await _context.Roles.FirstOrDefaultAsync(a => a.Name == request.Name && a.CooperativeId == request.CooperativeId);
                if (roledetails != null)
                {
                    return Result.Failure(new string[] { "Role details already exist" });
                }
                await _context.BeginTransactionAsync();
                var role = new CooperativeRole
                {
                    Name = request.Name.Trim(),
                    Cooperative = cooperative,
                    AccessLevel = request.AccessLevel,
                    AccessLevelDesc = request.AccessLevel.ToString(),
                    CreatedBy = user.user.Email,
                    CreatedById = user.user.UserId,
                    CreatedDate = DateTime.Now,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString()
                };

                await _context.Roles.AddAsync(role);
                await _context.SaveChangesAsync(cancellationToken);

                List<CooperativeRolePermission> permissions = new List<CooperativeRolePermission>();
                foreach (var permissionList in request.RolePermissions)
                {
                    foreach (var permission in permissionList.Value)
                    {
                        var rolePermission = new CooperativeRolePermission
                        {
                            CooperativeId = request.CooperativeId,
                            AccessLevel = request.AccessLevel,
                            Permission = permission.Permission.Trim(),
                            RoleId = role.Id,
                            CreatedBy = user.user.Email,
                            CreatedById = user.user.Id.ToString(),
                            CreatedDate = DateTime.Now,
                            Status = permission.Status,
                            Category = permissionList.Key
                        };
                        permissions.Add(rolePermission);
                    }
                }
                await _context.RolePermissions.AddRangeAsync(permissions);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();
                return Result.Success("Role Permission creation was successful");
            }
            catch (Exception ex)
            {
                _context.RollbackTransaction();
                return Result.Failure(new string[] { "Role and Permissions creation was not successful", ex?.Message ?? ex?.InnerException.Message });
            }

        }
    }
}
