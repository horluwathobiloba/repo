using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.Entities;

namespace RubyReloaded.AuthService.Application.PermissionSets.Queries.GetPermissionSets
{ 
    public class GetPermissionSetsQuery : IRequest<Result>
    {
        public int Skip { get; set; }
        public int Take { get; set; }
    }

    public class GetPermissionSetsQueryHandler : IRequestHandler<GetPermissionSetsQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetPermissionSetsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetPermissionSetsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var features = await _context.PermissionSets.Skip(request.Skip)
                                      .Take(request.Take).ToListAsync();
                var featureList = _mapper.Map<List<PermissionSetListDto>>(features);
                return Result.Success(featureList.OrderBy(a=>a.CreatedDate));
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving features was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
