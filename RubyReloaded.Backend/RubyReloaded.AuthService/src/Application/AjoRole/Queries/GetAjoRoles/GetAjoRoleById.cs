using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Application.Roles.Queries.GetRoles;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.AjoRole.Queries.GetAjoRoles
{
    public class GetAjoRoleById: IRequest<Result>
    {
        public int RoleId { get; set; }
    }
    public class GetAjoRoleByIdHandler : IRequestHandler<GetAjoRoleById, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        public GetAjoRoleByIdHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetAjoRoleById request, CancellationToken cancellationToken)
        {
            try
            {
                var role = await _context.AjoRoles.FirstOrDefaultAsync(a => a.Id == request.RoleId);
               // var roles = _mapper.Map<RoleDto>(role);
                return Result.Success(role);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving roles by Org Id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
  

}
