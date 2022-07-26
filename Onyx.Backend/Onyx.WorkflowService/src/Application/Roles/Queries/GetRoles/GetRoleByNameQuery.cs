using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Onyx.WorkFlowService.Application.Common.Interfaces;
using Onyx.WorkFlowService.Application.Common.Models;

namespace Onyx.WorkFlowService.Application.Roles.Queries.GetRoles
{
    public class GetRoleByNameQuery : IRequest<Result>
    {
        public string  Name { get; set; }
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
            var role = await  _context.Roles.FirstOrDefaultAsync(a=>a.Name == request.Name);
            var roles = _mapper.Map<RoleDto>(role);
            return Result.Success(roles);
           
        }
    }
}
