using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;

namespace RubyReloaded.AuthService.Application.Roles.Queries.GetRoles
{
    public class GetRoleByIdQuery : IRequest<Result>
    {
        public int  Id { get; set; }
    }

    public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetRoleByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var role = await _context.Roles.Include(a=>a.Cooperative).FirstOrDefaultAsync(a=>a.Id == request.Id);
                var roles = _mapper.Map<RoleDto>(role);
                return Result.Success(roles);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving roles by Org Id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
