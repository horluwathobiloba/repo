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

namespace OnyxDoc.AuthService.Application.Roles.Queries.GetRoles
{ 
    public class GetRolesQuery : IRequest<Result>
    {
        public int Skip { get; set; }
        public int Take { get; set; }
    }

    public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetRolesQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var roles = await _context.Roles.Include(a=>a.Subscriber).Skip(request.Skip)
                                      .Take(request.Take).ToListAsync();
                var roleList = _mapper.Map<List<RoleListDto>>(roles);
                return Result.Success(roleList.OrderBy(a=>a.CreatedDate));
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving roles was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
