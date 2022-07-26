using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Onyx.WorkFlowService.Application.Common.Interfaces;
using System.Linq;
using System.Collections.Generic;
using Onyx.WorkFlowService.Application.Common.Models;

namespace Onyx.WorkFlowService.Application.RolePermissions.Queries.GetRolePermissions
{
    public class GetRolesPermissionByRoleIdQuery : IRequest<Result>
    {
        public int RoleId { get; set; }
    }

    public class GetRolePermissionsByRoleIdHandler : IRequestHandler<GetRolesPermissionByRoleIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetRolePermissionsByRoleIdHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetRolesPermissionByRoleIdQuery request, CancellationToken cancellationToken)
        {
            var rolePermissions = await  _context.RolePermissions.Where(a=>a.RoleId == request.RoleId).ToListAsync();
            var roleList = _mapper.Map<List<RolePermissionListDto>>(rolePermissions);
            return Result.Success(roleList);
        }
    }
}
