using AutoMapper;
using MediatR;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.Utilities.Queries
{
    public class GetSubscriptionTypeEnums : AuthToken, IRequest<Result>
    {
       // public int SubscriberId { get; set; }
        public string UserId { get; set; }
    }

    public class GetSubscriptionTypeEnumsHandler : IRequestHandler<GetSubscriptionTypeEnums, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetSubscriptionTypeEnumsHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetSubscriptionTypeEnums request, CancellationToken cancellationToken)
        {
            try
            {
                return await Task.Run(() => Result.Success(
                   ((SubscriptionType[])Enum.GetValues(typeof(SubscriptionType))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                   )); 
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving subscription type enums. Error: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
