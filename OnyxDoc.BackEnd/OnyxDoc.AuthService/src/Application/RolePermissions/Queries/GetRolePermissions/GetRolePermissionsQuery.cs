using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Application.Common.Models;

namespace OnyxDoc.AuthService.Application.RolePermissions.Queries.GetRolePermissions
{ 
    public class GetRolePermissionsQuery : IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }

    public class GetRolesQueryHandler : IRequestHandler<GetRolePermissionsQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetRolesQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetRolePermissionsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var rolePermissions = await _context.RolePermissions.Skip(request.Skip)
                                      .Take(request.Take).ToListAsync();
              
                var permissions = _mapper.Map<List<RolePermissionListDto>>(rolePermissions);
                return Result.Success(permissions);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
