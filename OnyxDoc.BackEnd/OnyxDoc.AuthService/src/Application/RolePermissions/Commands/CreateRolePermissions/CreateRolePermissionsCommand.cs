using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using OnyxDoc.AuthService.Domain.Enums;
using System;
using OnyxDoc.AuthService.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using RestSharp;
using System.Collections.Generic;

namespace OnyxDoc.AuthService.Application.RolePermissions.Commands.CreateRolePermissions
{
    public class CreateRolePermissionsCommand : IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string UserId { get; set; }
        public int RoleId { get; set; }
        public RoleAccessLevel RoleAccessLevel { get; set; }
        public List<string> RolePermissions { get; set; }
    }

    public class CreateRoleCommandHandler : IRequestHandler<CreateRolePermissionsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly ISqlService _sqlService;

        public CreateRoleCommandHandler(IApplicationDbContext context, IIdentityService identityService, ISqlService sqlService)
        {
            _context = context;
            _identityService = identityService;
            _sqlService = sqlService;
        }

        public async Task<Result> Handle(CreateRolePermissionsCommand request, CancellationToken cancellationToken)
        {
            try
            {

                var user = await _identityService.GetUserByIdAndSubscriber(request.UserId, request.SubscriberId);
                if (user.user == null)
                {
                    return Result.Failure(new string[] { "Unable to update role.Invalid User ID and Subscriber credentials!" });
                }

                if (request.RolePermissions == null || request.RolePermissions.Count == 0)
                {
                    return Result.Failure(new string[] { "Please input valid Role Permissions", });
                }
                List<RolePermission> permissions = new List<RolePermission>();
                foreach (var permission in request.RolePermissions)
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
                    permissions.Add(rolePermissions);
                   // await _context.RolePermissions.AddAsync(rolePermissions);
                }
                await _context.RolePermissions.AddRangeAsync(permissions);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Role Permission creation was successful", permissions);
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Role Permission creation was not successful", ex?.Message??ex?.InnerException.Message });
            }

        }
    }
}
