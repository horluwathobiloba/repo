using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.AjoRolePermission.Queries.GetAjoRolePermissions
{
    public class GetAjoRolePermmisionByRoleIdCommand:IRequest<Result>
    {
        public int RoleId { get; set; }
       
        public AccessLevel AccessLevel { get; set; }
    }
    public class GetAjoRolePermmisionByRoleIdCommandHandler : IRequestHandler<GetAjoRolePermmisionByRoleIdCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;
        public GetAjoRolePermmisionByRoleIdCommandHandler(IApplicationDbContext context, IIdentityService identityService, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _identityService = identityService;
        }
        public async Task<Result> Handle(GetAjoRolePermmisionByRoleIdCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var role = await _context.AjoRoles.FirstOrDefaultAsync(x => x.Id == request.RoleId);
                if (role is null)
                {
                    return Result.Failure("Invalid role Id");
                }
                var permissions = await _context.AjoRolePermissions.Where(x => x.RoleId == role.Id).ToListAsync();
                var category = permissions.Select(a => a.Category).Distinct().ToList();
                var permissionsDictionary = new Dictionary<string, List<Domain.Entities.AjoRolePermission>>();
                foreach (var item in category)
                {
                    permissionsDictionary.Add(item, permissions.Where(a => a.Category == item).ToList());
                }
                return Result.Success(new { role, permissionsDictionary });
            }
            catch (Exception ex)
            {
                return Result.Failure("Retrieving roles and permissions by Role Id was not successful" + ex?.Message ?? ex?.InnerException?.Message);
            } 
        }
    }
}
