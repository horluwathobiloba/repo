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
    public class GetCurrencyEnums : AuthToken, IRequest<Result>
    {
        //  public int SubscriberId { get; set; }
        public string UserId { get; set; }
    }
    public class GetCurrencyEnumsHandler : IRequestHandler<GetCurrencyEnums, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetCurrencyEnumsHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetCurrencyEnums request, CancellationToken cancellationToken)
        {
            try
            {
                return await Task.Run(() => Result.Success(
                    ((CurrencyCode[])Enum.GetValues(typeof(CurrencyCode))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                    ));
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving currency enums. Error: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
