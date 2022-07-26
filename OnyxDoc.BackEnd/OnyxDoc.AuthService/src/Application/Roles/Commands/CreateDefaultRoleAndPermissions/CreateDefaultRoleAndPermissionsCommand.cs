using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Application.Common.Models;
using OnyxDoc.AuthService.Application.RolePermissions.Commands.CreateRolePermissions;
using OnyxDoc.AuthService.Domain.Entities;
using OnyxDoc.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.AuthService.Application.Roles.Commands.CreateDefaultRoleAndPermissions
{
    public class CreateDefaultRoleAndPermissionsCommand : IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public DefaultRoles RoleName { get; set; }
        public SubscriberType SubscriberType { get; set; }
        public RoleAccessLevel RoleAccessLevel { get; set; }
        public Dictionary<string, List<RolePermissionsVm>> RolePermissions { get; set; }


    }

    public class CreateDefaultRoleAndPermissionsCommandHandler : IRequestHandler<CreateDefaultRoleAndPermissionsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly ISqlService _sqlService;

        public CreateDefaultRoleAndPermissionsCommandHandler(IApplicationDbContext context,
            IIdentityService identityService, ISqlService sqlService)
        {
            _context = context;
            _identityService = identityService;
            _sqlService = sqlService;
        }

        public async Task<Result> Handle(CreateDefaultRoleAndPermissionsCommand request, CancellationToken cancellationToken)
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

                var checkDefaultRoleIfExist =  _context.DefaultRolesConfigurations.Where(x => x.Subscriber.Id == subscriber.Id && x.RoleName == request.RoleName);
                if (checkDefaultRoleIfExist.Count() > 0)
                {
                    return Result.Failure("Default role already created");
                }

                var defaultRole = new DefaultRolesConfiguration
                {
                    Name = request.Name,
                    CreatedByEmail = userCheck.user.Email,
                    CreatedById = request.UserId,
                    CreatedDate = DateTime.Now,
                    RoleName = request.RoleName,
                    SubscriberType = request.SubscriberType,
                    SubscriberId = request.SubscriberId,
                    RoleAccessLevel = request.RoleAccessLevel,
                    RoleAccessLevelDesc = request.RoleAccessLevel.ToString(),
                    Status = Status.Active, //Should it be active by default?
                    StatusDesc = Status.Active.ToString(), //Should this also be active by default?
                    LastModifiedByEmail = userCheck.user.Email,
                    LastModifiedById = request.UserId,
                    LastModifiedDate = DateTime.Now,
                    

                };
                await _context.DefaultRolesConfigurations.AddAsync(defaultRole);
                await _context.SaveChangesAsync(cancellationToken);

                var rolePermissionList = new List<RolePermission>();
                if (defaultRole.Id > 0)
                {
                    foreach (var permissionList in request.RolePermissions)
                    {
                        foreach (var permission  in permissionList.Value)
                        {
                            var rolePermission = new RolePermission
                            {
                                Name = defaultRole.Name.ToString(),
                                SubscriberId = request.SubscriberId,
                                RoleAccessLevel = request.RoleAccessLevel,
                                Permission = permission.Permission.Trim(),
                                RoleId = defaultRole.Id,
                                CreatedByEmail =userCheck.user?.UserId,
                                CreatedById= request.UserId,
                                CreatedDate= DateTime.Now,
                                Status = permission.Status,
                                StatusDesc = permission.Status.ToString(),
                                Category = permissionList.Key,
                                LastModifiedByEmail = userCheck.user.Email,
                                LastModifiedById = request.UserId,
                                LastModifiedDate = DateTime.Now
                            };

                            rolePermissionList.Add(rolePermission);
                        }
                    }
                }
               
                await _context.RolePermissions.AddRangeAsync(rolePermissionList);
                await _context.SaveChangesAsync(cancellationToken);


                return Result.Success("Default Role and it permissions successfully created");
            }
            catch (Exception ex)
            {
                _context.RollbackTransaction();
                return Result.Failure(new string[] { "Default Role and Permissions creation was not successful", ex?.Message ?? ex?.InnerException.Message });

            }
        }
    }
}
