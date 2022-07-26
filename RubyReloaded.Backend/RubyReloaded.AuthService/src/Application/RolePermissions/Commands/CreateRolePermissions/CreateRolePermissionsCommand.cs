using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using RubyReloaded.AuthService.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using RestSharp;
using System.Collections.Generic;
using RubyReloaded.AuthService.Domain.ViewModels;

namespace RubyReloaded.AuthService.Application.RolePermissions.Commands.CreateRolePermissions
{
    public class CreateRolePermissionsCommand : IRequest<Result>
    {
        public int CooperativeId { get; set; }
        public string UserId { get; set; }
        public int RoleId { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public Dictionary<string, List<RolePermissionsVm>> RolePermissions { get; set; }
    }

    public class CreateRoleCommandHandler : IRequestHandler<CreateRolePermissionsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public CreateRoleCommandHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<Result> Handle(CreateRolePermissionsCommand request, CancellationToken cancellationToken)
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
                var getRole = await _context.Roles.FirstOrDefaultAsync(a => a.Id == request.RoleId);
                if (getRole == null)
                {
                    return Result.Failure("Role Cannot be Found");
                }
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
                            RoleId = request.RoleId,
                            
                            CreatedBy = user.user.Email,
                            CreatedById = user.user.Id.ToString(),
                            CreatedDate = DateTime.Now,
                            Status = permission.Status,
                            Category=permissionList.Key
                            
                        };
                        permissions.Add(rolePermission);
                    }
                }
                await _context.RolePermissions.AddRangeAsync(permissions);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Role Permission creation was successful");
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Role Permission creation was not successful", ex?.Message??ex?.InnerException.Message });
            }

        }
    }
}
