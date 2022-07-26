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

namespace OnyxDoc.AuthService.Application.Features.Queries.GetFeatures
{ 
    public class GetFeaturesQuery : IRequest<Result>
    {
        public int Skip { get; set; }
        public int Take { get; set; }
    }

    public class GetFeaturesQueryHandler : IRequestHandler<GetFeaturesQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetFeaturesQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetFeaturesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var features = await _context.Features.Skip(request.Skip)
                                      .Take(request.Take).ToListAsync();
                var featureList = _mapper.Map<List<FeatureListDto>>(features);
                return Result.Success(featureList.OrderBy(a=>a.CreatedDate));
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving features was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
