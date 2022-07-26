using AutoMapper;
using MediatR;
using RubyReloaded.SubscriptionService.Application.Common.Interfaces;
using RubyReloaded.SubscriptionService.Application.Common.Models;
using RubyReloaded.SubscriptionService.Domain.Enums;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.SubscriptionService.Application.Utilities.Queries
{
    public class GetSubscriptionFrequencyEnums : AuthToken, IRequest<Result>
    {
        // public int SubscriberId { get; set; }
        public string UserId { get; set; }
    }

    public class GetSubscriptionFrequencyEnumsHandler : IRequestHandler<GetSubscriptionFrequencyEnums, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetSubscriptionFrequencyEnumsHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetSubscriptionFrequencyEnums request, CancellationToken cancellationToken)
        {
            try
            {
                return await Task.Run(() => Result.Success(
                   ((SubscriptionFrequency[])Enum.GetValues(typeof(SubscriptionFrequency))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                   ));
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving subscription plan enums. Error: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
