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

namespace OnyxDoc.AuthService.Application.Features.Queries.GetFeatures
{
    public class GetFeatureByNameQuery : IRequest<Result>
    {
        public string Name { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }

    public class GetFeatureByNameQueryHandler : IRequestHandler<GetFeatureByNameQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetFeatureByNameQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetFeatureByNameQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var feature = await _context.Features.Where(a => a.Name == request.Name).Skip(request.Skip)
                                      .Take(request.Take).ToListAsync(); ;
                var features = _mapper.Map<List<FeatureDto>>(feature);
                return Result.Success(features);
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Retrieving features by name was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }

        }






    }
}
