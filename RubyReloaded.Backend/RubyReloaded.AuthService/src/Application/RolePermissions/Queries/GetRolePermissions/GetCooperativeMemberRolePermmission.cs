using MediatR;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace RubyReloaded.AuthService.Application.RolePermissions.Queries.GetRolePermissions
{
    public class GetCooperativeMemberRolePermmission : IRequest<Result>
    {
        public string Email { get; set; }
        public int CooperativeId { get; set; }
    }
    public class GetCooperativeMemberRolePermmissionHandler : IRequestHandler<GetCooperativeMemberRolePermmission, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly ISqlService _sqlService;
        public GetCooperativeMemberRolePermmissionHandler(IApplicationDbContext context, IIdentityService identityService, ISqlService sqlService)
        {
            _context = context;
            _identityService = identityService;
            _sqlService = sqlService;
        }
        public async Task<Result> Handle(GetCooperativeMemberRolePermmission request, CancellationToken cancellationToken)
        {
            try
            {
                var permissions = await (from x in _context.CooperativeMembers
                                         .Where(a => a.Email == request.Email && a.CooperativeId == request.CooperativeId)
                                         .DefaultIfEmpty()
                                         join y in _context.RolePermissions
                                         on x.RoleId equals y.RoleId into permList
                                         from p in permList
                                         select new
                                         {
                                             id = p.Id,
                                             cooperativeId = p.CooperativeId,
                                             name = p.Name,
                                             roleId = p.RoleId,
                                              status = p.Status,
                                              category=p.Category
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
