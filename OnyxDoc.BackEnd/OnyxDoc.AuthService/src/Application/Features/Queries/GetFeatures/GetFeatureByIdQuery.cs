using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Application.Common.Models;

namespace OnyxDoc.AuthService.Application.Features.Queries.GetFeatures
{
    public class GetFeatureByIdQuery : IRequest<Result>
    {
        public int  Id { get; set; }
    }

    public class GetFeatureByIdQueryHandler : IRequestHandler<GetFeatureByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetFeatureByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetFeatureByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var feature = await _context.Features.FirstOrDefaultAsync(a=>a.Id == request.Id);
                var features = _mapper.Map<FeatureDto>(feature);
                return Result.Success(features);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving features by Id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
