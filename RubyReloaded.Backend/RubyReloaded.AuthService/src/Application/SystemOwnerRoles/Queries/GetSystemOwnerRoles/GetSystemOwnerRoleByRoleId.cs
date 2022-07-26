using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.SystemOwnerRoles.Queries.GetSystemOwnerRoles
{
    public class GetSystemOwnerRoleByRoleId:IRequest<Result>
    {
        public int RoleId { get; set; }
    }
    public class GetSystemOwnerRoleByRoleIdHandler : IRequestHandler<GetSystemOwnerRoleByRoleId, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        public GetSystemOwnerRoleByRoleIdHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result> Handle(GetSystemOwnerRoleByRoleId request, CancellationToken cancellationToken)
        {

            try
            {
                var role = await _context.SystemOwnerRoles.FirstOrDefaultAsync(a => a.Id == request.RoleId);
                return Result.Success(role);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving roles by Org Id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
