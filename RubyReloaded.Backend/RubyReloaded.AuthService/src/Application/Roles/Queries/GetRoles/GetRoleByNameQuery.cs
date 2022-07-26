using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using System.Collections.Generic;

namespace RubyReloaded.AuthService.Application.Roles.Queries.GetRoles
{
    public class GetRoleByNameQuery : IRequest<Result>
    {
        public string Name { get; set; }
    }

    public class GetRoleByNameQueryHandler : IRequestHandler<GetRoleByNameQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetRoleByNameQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetRoleByNameQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var role = await _context.Roles.Include(a=>a.Cooperative).Where(a => a.Name == request.Name).ToListAsync();
                //var role = await _context.Roles.FirstOrDefaultAsync(a => a.Name == request.Name);
                var roles = _mapper.Map<List<RoleDto>>(role);
                return Result.Success(roles);
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Retrieving roles by Org Id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }

        }






    }
}
