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

namespace RubyReloaded.AuthService.Application.AjoRolePermission.Queries.GetAjoRolePermissions
{
    public class GetAjoMemberRolePermissions : IRequest<Result>
    {
        public string Email { get; set; }
        public int AjoId { get; set; }
    }
    public class GetAjoMemberRolePermissionsHandler : IRequestHandler<GetAjoMemberRolePermissions, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly ISqlService _sqlService;
        public GetAjoMemberRolePermissionsHandler(IApplicationDbContext context, IIdentityService identityService, ISqlService sqlService)
        {
            _context = context;
            _identityService = identityService;
            _sqlService = sqlService;
        }
        public async  Task<Result> Handle(GetAjoMemberRolePermissions request, CancellationToken cancellationToken)
        {
            try
            {
                var permissions = await(from x in _context.AjoMembers
                                        .Where(a => a.Email == request.Email && a.AjoId == request.AjoId)
                                        .DefaultIfEmpty()
                                        join y in _context.AjoRolePermissions
                                        on x.RoleId equals y.RoleId into permList
                                        from p in permList
                                        select new
                                        {
                                            id = p.Id,
                                            ajoid = p.AjoId,
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
