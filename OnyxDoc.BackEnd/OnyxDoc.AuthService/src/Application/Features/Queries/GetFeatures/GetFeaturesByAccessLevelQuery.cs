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
using OnyxDoc.AuthService.Domain.Enums;

namespace OnyxDoc.AuthService.Application.Features.Queries.GetFeatures
{
    public class GetFeaturesByAccessLevelQuery : IRequest<Result>
    {
        public AccessLevel AccessLevel { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }

    public class GetFeaturesByAccessLevelQueryHandler : IRequestHandler<GetFeaturesByAccessLevelQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetFeaturesByAccessLevelQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetFeaturesByAccessLevelQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var features = await _context.Features.Where(a=>a.AccessLevel == request.AccessLevel).Skip(request.Skip)
                                      .Take(request.Take).ToListAsync();
                var featureLists = _mapper.Map<List<FeatureListDto>>(features);
                return Result.Success(featureLists);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving features by access level was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
   