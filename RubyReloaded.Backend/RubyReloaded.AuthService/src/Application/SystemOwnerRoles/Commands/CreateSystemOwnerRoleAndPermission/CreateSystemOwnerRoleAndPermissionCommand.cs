using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Application.RolePermissions.Queries.GetRolePermissions;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.SystemOwnerRoles.Commands.CreateSsystemOwnerRoleAndPermission
{
    public class PermViewModel
    {
        public Status Status { get; set; }
        public string Permission { get; set; }
    }
    public class CreateSystemOwnerRoleAndPermissionCommand:IRequest<Result>
    {
        public int SystemOwnerId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public AccessLevel AccessLevel { get; set; }
        //public Dictionary<string, List<PermissionViewModel>> RolePermissions { get; set; }
        //public List<PermissionViewModel> RolePermissions { get; set; }
        public Dictionary<string,List<PermViewModel>> RolePermissions { get; set; }
    }
    public class CreateSystemOwnerRoleAndPermissionCommandHandler : IRequestHandler<CreateSystemOwnerRoleAndPermissionCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        public CreateSystemOwnerRoleAndPermissionCommandHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;

        }
        
        public async Task<Result> Handle(CreateSystemOwnerRoleAndPermissionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _context.BeginTransactionAsync();
                var systemOwner = await _context.SystemOwners.FirstOrDefaultAsync();
                var role = new Domain.Entities.SystemOwnerRole
                {
                    Name = request.Name,
                    CreatedById = request.UserId,
                    CreatedDate = DateTime.Now,
                    AccessLevel = request.AccessLevel,
                    AccessLevelDesc = request.AccessLevel.ToString(),
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString(),
                    SystemOwnerId = request.SystemOwnerId,
                    SystemOwner=systemOwner
                };
                await _context.SystemOwnerRoles.AddAsync(role);
                await _context.SaveChangesAsync(cancellationToken);
                var rolePermissionsList = new List<Domain.Entities.SystemOwnerRolePermission>();
                if (role.Id != 0)
                {
                    foreach (var permissionList in request.RolePermissions)
                    {
                        foreach (var permission in permissionList.Value)
                        {
                            var rolePermission = new Domain.Entities.SystemOwnerRolePermission
                            {
                                AccessLevel = request.AccessLevel,
                                Permission = permission.Permission.Trim(),
                                RoleId = role.Id,

                                CreatedById = request.UserId,
                                CreatedDate = DateTime.Now,
                                Status = permission.Status,
                                StatusDesc = permission.Status.ToString(),
                                Category = permissionList.Key
                            };

                            rolePermissionsList.Add(rolePermission);
                        }
                    }
                }
                await _context.SystemOwnerRolePermissions.AddRangeAsync(rolePermissionsList);
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
