using MediatR;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Application.RolePermissions.Queries.GetRolePermissions;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.AjoRole.Commands.CreateAjoRoleAndPermissons
{

    public class PermissionViewModel
    {
        public Status Status { get; set; }
        public string Permission { get; set; }
    }
    public class CreateRoleAndPermissonAjoCommand : IRequest<Result>
    {
        public int AjoId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public Dictionary<string,List<PermissionViewModel>> RolePermissions { get; set; }
        
    }
    public class CreateAjoRoleAndPermissonCommandHandler : IRequestHandler<CreateRoleAndPermissonAjoCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public CreateAjoRoleAndPermissonCommandHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;

        }

        public async Task<Result> Handle(CreateRoleAndPermissonAjoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _context.BeginTransactionAsync();
                var role = new Domain.Entities.AjoRole
                {
                    Name = request.Name,
                    CreatedById = request.UserId,
                    CreatedDate = DateTime.Now,
                    AccessLevel = request.AccessLevel,
                    AccessLevelDesc = request.AccessLevel.ToString(),
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString(),
                    AjoId = request.AjoId
                };
                await _context.AjoRoles.AddAsync(role);
                await _context.SaveChangesAsync(cancellationToken);
                var rolePermissionsList = new List<Domain.Entities.AjoRolePermission>();
                if (role.Id != 0)
                {
                    foreach (var permissionList in request.RolePermissions)
                    {
                        foreach (var permission in permissionList.Value)
                        {
                            var rolePermission = new Domain.Entities.AjoRolePermission
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
                await _context.AjoRolePermissions.AddRangeAsync(rolePermissionsList);
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
