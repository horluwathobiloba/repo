using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using OnyxDoc.AuthService.Application.Common.Interfaces;
using System.Linq;
using System.Collections.Generic;
using OnyxDoc.AuthService.Application.Common.Models;
using OnyxDoc.AuthService.Domain.Enums;
using OnyxDoc.AuthService.Domain.Entities;
using System.Text.RegularExpressions;
using System;

namespace OnyxDoc.AuthService.Application.RolePermissions.Queries.GetRolePermissions
{
    public class GetRolesPermissionByRoleIdQuery : IRequest<Result>
    {
        public int RoleId { get; set; }
        public Status Status { get; set; }
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
            return Result.Success();
        }
    }
}
