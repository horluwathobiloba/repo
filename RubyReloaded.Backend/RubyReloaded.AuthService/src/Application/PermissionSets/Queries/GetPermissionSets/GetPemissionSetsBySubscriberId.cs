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
    public class GetPermissionSetsBySubscriberIdQuery : IRequest<Result>
    {
        public AccessLevel AccessLevel { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }

    public class GetPermissionSetsBySubscriberIdQueryHandler : IRequestHandler<GetPermissionSetsBySubscriberIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetPermissionSetsBySubscriberIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetPermissionSetsBySubscriberIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                //TODO : get subscriptions features instead
                var features = await _context.PermissionSets.Where(a=>a.AccessLevel == request.AccessLevel).Skip(request.Skip)
                                      .Take(request.Take).ToListAsync();
                var featureLists = _mapper.Map<List<PermissionSetListDto>>(features);
                return Result.Success(featureLists.OrderBy(a=>a.CreatedDate));
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving features by subscriber id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
   