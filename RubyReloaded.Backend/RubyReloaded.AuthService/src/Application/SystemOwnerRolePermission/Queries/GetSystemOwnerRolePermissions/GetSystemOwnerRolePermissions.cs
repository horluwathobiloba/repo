using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.SystemOwnerRolePermission.Queries.GetSystemOwnerRolePermissions
{
    public class GetSystemOwnerRolePermissions:IRequest<Result>
    {
        public string Email { get; set; }
        public int SystemOwnerUserId { get; set; }
    }

    public class GetSystemOwnerRolePermissionsHandler : IRequestHandler<GetSystemOwnerRolePermissions, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly ISqlService _sqlService;
        public GetSystemOwnerRolePermissionsHandler(IApplicationDbContext context, IIdentityService identityService, ISqlService sqlService)
        {
            _context = context;
            _identityService = identityService;
            _sqlService = sqlService;
        }
        public async Task<Result> Handle(GetSystemOwnerRolePermissions request, CancellationToken cancellationToken)
        {
            try
            {
                var permissions = await (from x in _context.SystemOwnerUsers
                                         .Where(a => a.Email == request.Email && a.SystemOwnerId == request.SystemOwnerUserId)
                                         .DefaultIfEmpty()
                                         join y in _context.SystemOwnerRolePermissions
                                         on x.RoleId equals y.RoleId into permList
                                         from p in permList
                                         select new
                                         {
                                             id = p.Id,  
                                             ajoid = p.SystemOwnerId,
                                             name = p.Name,
                                             roleId = p.RoleId,
                                             status = p.Status,
                                             category = p.Category
                                         }).ToListAsync();

                return Result.Success(permissions);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Role permission retrieval was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }

}
