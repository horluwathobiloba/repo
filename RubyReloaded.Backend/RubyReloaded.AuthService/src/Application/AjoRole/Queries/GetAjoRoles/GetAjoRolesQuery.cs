using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Application.Roles.Queries.GetRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.AjoRole.Queries.GetAjoRoles
{
    public class GetAjoRolesQuery:IRequest<Result>
    {
        public int Skip { get; set; }
        public int Take { get; set; }
        public int AjoId { get; set; }
    }
    public class GetAjoRolesQueryHandler : IRequestHandler<GetAjoRolesQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        public GetAjoRolesQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result> Handle(GetAjoRolesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var roles = await _context.AjoRoles.Skip(request.Skip)
                                                          .Take(request.Take).ToListAsync();
                var roleList = _mapper.Map<List<RoleListDto>>(roles);
                return Result.Success(roleList.OrderBy(a => a.CreatedDate));
            }
            catch (Exception ex)
            {
             return Result.Failure(new string[] { "Role and Permissions creation was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
