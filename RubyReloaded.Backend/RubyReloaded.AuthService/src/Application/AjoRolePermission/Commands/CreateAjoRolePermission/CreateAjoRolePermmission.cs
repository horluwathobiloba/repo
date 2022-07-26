﻿using MediatR;
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

namespace RubyReloaded.AuthService.Application.AjoRolePermission.CreateAjoRolePermission
{
    public class CreateAjoRolePermmission:IRequest<Result>
    {
        public int AjoId { get; set; }
        public string UserId { get; set; }
        public int RoleId { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public Dictionary<string, List<RolePermissionsVm>> RolePermissions { get; set; }
    }

    public class CreateAjoRolePermmissionHandler : IRequestHandler<CreateAjoRolePermmission, Result>
    {

        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        public CreateAjoRolePermmissionHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }
        public async Task<Result> Handle(CreateAjoRolePermmission request, CancellationToken cancellationToken)
        {
            try
            {

                //var user = await _identityService.GetUserByIdAndOrganization(request.UserId, request.OrganizationId);
                //if (user.user == null)
                //{
                //    return Result.Failure(new string[] { "Unable to create role permissions.Invalid User ID and Organization credentials!" });
                //}
                //if (request.RolePermissions == null || request.RolePermissions.Count == 0)
                //{
                //    return Result.Failure(new string[] { "Please input valid role permissions", });
                //}
                var user = await _identityService.GetUserById(request.UserId);
                var getRolePermissions = await _context.AjoRolePermissions.Where(a => a.RoleId == request.RoleId).ToListAsync();
                List<Domain.Entities.AjoRolePermission> permissions = new List<Domain.Entities.AjoRolePermission>();
                foreach (var permissionList in request.RolePermissions)
                {
                    foreach (var permission in permissionList.Value)
                    {
                        var rolePermission = new Domain.Entities.AjoRolePermission
                        {
                            AjoId = request.AjoId,
                            AccessLevel = request.AccessLevel,
                            Permission = permission.Permission.Trim(),
                            RoleId = request.RoleId,
                            CreatedBy = user.user.Email,
                            CreatedById = user.user.Id.ToString(),
                            CreatedDate = DateTime.Now,
                            Status = Status.Active,
                            Category=permissionList.Key
                        };
                        permissions.Add(rolePermission);
                    }
                   
                }
                await _context.AjoRolePermissions.AddRangeAsync(permissions);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Role Permission creation was successful");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Role Permission creation was not successful", ex?.Message ?? ex?.InnerException.Message });
            }

        }
    }
}
