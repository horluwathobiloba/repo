using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OnyxDoc.AuthService.Application.Common.Interfaces;
using System.Collections.Generic;
using OnyxDoc.AuthService.Application.Common.Models;

namespace OnyxDoc.AuthService.Application.Roles.Queries.GetRoles
{
    public class GetRolesBySubscriberIdQuery : IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }

    public class GetRolesBySubscriberIdQueryHandler : IRequestHandler<GetRolesBySubscriberIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetRolesBySubscriberIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetRolesBySubscriberIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var roles = await _context.Roles.Include(a=>a.Subscriber).Where(a=>a.SubscriberId == request.SubscriberId).Skip(request.Skip)
                                      .Take(request.Take).ToListAsync();
                var roleLists = _mapper.Map<List<RoleListDto>>(roles);
                return Result.Success(roleLists.OrderBy(a=>a.CreatedDate));
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving roles by Org Id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
   