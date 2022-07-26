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
    public class GetFeaturesBySubscriberIdQuery : IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }

    public class GetFeaturesBySubscriberIdQueryHandler : IRequestHandler<GetFeaturesBySubscriberIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetFeaturesBySubscriberIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetFeaturesBySubscriberIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                //TODO : get subscriptions features instead
                var features = await _context.Features.Where(a=>a.AccessLevel == request.AccessLevel && a.SubscriberId == request.SubscriberId).Skip(request.Skip)
                                      .Take(request.Take).ToListAsync();
                var featureLists = _mapper.Map<List<FeatureListDto>>(features);
                return Result.Success(featureLists.OrderBy(a=>a.CreatedDate));
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving features by subscriber id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
   