using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Application.Common.Models;
using System.Collections.Generic;

namespace OnyxDoc.AuthService.Application.Roles.Queries.GetRoles
{
    public class GetRolesForDisplayQuery : IRequest<Result>
    {
        public int SubscriberId { get; set; }
    }

    public class GetRolesForDisplayQueryHandler : IRequestHandler<GetRolesForDisplayQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetRolesForDisplayQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetRolesForDisplayQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var role = await _context.Roles.Where(a => a.SubscriberId == request.SubscriberId).OrderBy(a=>a.CreatedDate).Take(3).ToListAsync();
                var roles = _mapper.Map<List<RoleDto>>(role);
                return Result.Success(roles);
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Retrieving roles for display was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }

        }






    }
}
