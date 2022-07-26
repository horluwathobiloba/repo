using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using OnyxDoc.AuthService.Application.Common.Models;
using OnyxDoc.AuthService.Domain.Enums;
using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Domain.Entities;
using OnyxDoc.AuthService.Domain.ViewModels;
using System.Linq;

namespace OnyxDoc.AuthService.Application.Roles.Commands.CreateRoleAndPermissions
{
    public class CreateRoleAndPermissionsCommand : IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public RoleAccessLevel RoleAccessLevel { get; set; }
        public Dictionary<string, List<RolePermissionsVm>> RolePermissions { get; set; }
        public int CheckedCount { get; set; }
    }

    public class CreateRoleAndPermissionsCommandHandler : IRequestHandler<CreateRoleAndPermissionsCommand, Result>
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
      
        public async Task<Result> Handle(CreateRoleAndPermissionsCommand request, CancellationToken cancellationToken)
        {
            try
            {

                var subscriber = await _context.Subscribers.Where(a => a.Id == request.SubscriberId).FirstOrDefaultAsync();
                if (subscriber == null)
                {
                    return Result.Failure("Invalid Subscriber Specified");
                }
                var userCheck = await _identityService.GetUserByIdAndSubscriber(request.UserId, request.SubscriberId);
                if (userCheck.user == null)
                {
                    return Result.Failure("User does not exist in this organisation");
                }
                var role = new Role
                {
                    Name = request.Name,
                    CreatedByEmail = userCheck.user?.UserId,
                    CreatedById = request.UserId,
                    CreatedDate = DateTime.Now,
                    RoleAccessLevel = request.RoleAccessLevel,
                    RoleAccessLevelDesc = request.RoleAccessLevel.ToString(),
                    SubscriberId = request.SubscriberId,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString(),
                    CheckedCount = request.CheckedCount
                };
                await _context.Roles.AddAsync(role);
                await _context.SaveChangesAsync(cancellationToken);
                var rolePermissionsList = new List<RolePermission>();
                if (role.Id != 0)
                {
                    foreach (var permissionList in request.RolePermissions)
                    {
                        foreach (var permission in permissionList.Value)
                        {
                            var rolePermission = new RolePermission
                            {

                                SubscriberId = request.SubscriberId,
                                RoleAccessLevel = request.RoleAccessLevel,
                                Permission = permission.Permission.Trim(),
                                RoleId = role.Id,
                                CreatedByEmail = userCheck.user?.UserId,
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
                await _context.RolePermissions.AddRangeAsync(rolePermissionsList);
                await _context.SaveChangesAsync(cancellationToken);
                //await _sqlService.InsertPermissions("RolePermissions", rolePermissionsList);

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
