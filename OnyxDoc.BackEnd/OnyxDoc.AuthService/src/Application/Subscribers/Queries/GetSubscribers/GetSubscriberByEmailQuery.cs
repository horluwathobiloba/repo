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
    public class GetSubscriberBySubscribernameQuery : IRequest<Result>
    {
        public string Email { get; set; }
    }

    public class GetSubscriberBySubscribernameQueryHandler : IRequestHandler<GetSubscriberBySubscribernameQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetSubscriberBySubscribernameQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetSubscriberBySubscribernameQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var subscriber = await _context.Subscribers.Where(a => a.ContactEmail == request.Email).FirstOrDefaultAsync();
                var subscriberDetails = _mapper.Map<SubscriberDto>(subscriber);
                if (subscriberDetails == null)
                {
                    return Result.Failure(new string[] { $"Subscriber Details does not exist with this email: {request.Email}" });
                }
                return Result.Success(subscriberDetails);
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Error retrieving subscriber by email: ", ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
