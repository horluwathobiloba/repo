using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Application.Common.Models;

namespace OnyxDoc.AuthService.Application.Subscribers.Queries.GetSubscribers
{
    public class GetSubscriberByIdQuery : IRequest<Result>
    {
        public int Id { get; set; }

        public string UserId { get; set; }
    }

    public class GetSubscriberByIdQueryHandler : IRequestHandler<GetSubscriberByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public GetSubscriberByIdQueryHandler(IApplicationDbContext context, IIdentityService identityService, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _identityService = identityService;
        }

        public async Task<Result> Handle(GetSubscriberByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var subscriber = await _context.Subscribers.Where(a => a.Id == request.Id).FirstOrDefaultAsync();
                var subscriberDetails = _mapper.Map<SubscriberDto>(subscriber);
                if (subscriberDetails == null)
                {
                    return Result.Failure(new string[] { $"Subscriber Details does not exist with Id: {request.Id}" });
                }
                
                return Result.Success(subscriberDetails);
                

            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Error retrieving subscriber details by Id: ", ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
