using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using System.Collections.Generic;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.Enums;

namespace RubyReloaded.AuthService.Application.PermissionSets.Queries.GetPermissionSets
{
    public class GetPermissionSetsByAccessLevelQuery : IRequest<Result>
    {
        public AccessLevel AccessLevel { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }

    public class GetPermissionSetsByAccessLevelQueryHandler : IRequestHandler<GetPermissionSetsByAccessLevelQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetPermissionSetsByAccessLevelQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetPermissionSetsByAccessLevelQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var features = await _context.PermissionSets.Where(a=>a.AccessLevel == request.AccessLevel).Skip(request.Skip)
                                      .Take(request.Take).ToListAsync();
                var featureLists = _mapper.Map<List<PermissionSetListDto>>(features);
                return Result.Success(featureLists);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving features by access level was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
   