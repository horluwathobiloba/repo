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
using OnyxDoc.AuthService.Domain.Entities;

namespace OnyxDoc.AuthService.Application.Subscribers.Queries.GetSubscribers
{
    public class GetSubscribersQuery : IRequest<Result>
    {
        public int Skip { get; set; }
        public int Take { get; set; }
    }

    public class GetSubscribersQueryHandler : IRequestHandler<GetSubscribersQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public GetSubscribersQueryHandler(IApplicationDbContext context, IIdentityService identityService, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _identityService = identityService;
        }

        public async Task<Result> Handle(GetSubscribersQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var subscriberList = new List<Subscriber>();
                if (request.Skip == 0 || request.Take == 0)
                {
                     subscriberList = await _context.Subscribers.ToListAsync();
                }
                    subscriberList = await _context.Subscribers.Skip(request.Skip)
                                      .Take(request.Take).ToListAsync();
                if (subscriberList == null)
                {
                    return Result.Failure("No subscribers exist");
                }
                var result  = _mapper.Map<List<SubscriberListDto>>(subscriberList);
               
                return Result.Success(result);
                  
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
