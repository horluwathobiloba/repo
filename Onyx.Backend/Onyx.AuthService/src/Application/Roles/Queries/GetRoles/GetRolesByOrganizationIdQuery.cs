using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Onyx.AuthService.Application.Common.Interfaces;
using System.Collections.Generic;
using Onyx.AuthService.Application.Common.Models;

namespace Onyx.AuthService.Application.Roles.Queries.GetRoles
{
    public class GetRolesByOrganizationIdQuery : IRequest<Result>
    {
        public int OrganizationId { get; set; }
    }

    public class GetRolesByOrganizationIdQueryHandler : IRequestHandler<GetRolesByOrganizationIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetRolesByOrganizationIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetRolesByOrganizationIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var roles = await _context.Roles.Include(a=>a.Organization).Where(a=>a.OrganizationId == request.OrganizationId).ToListAsync();
                var roleLists = _mapper.Map<List<RoleListDto>>(roles);
                return Result.Success(roleLists);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving roles by Org Id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
   