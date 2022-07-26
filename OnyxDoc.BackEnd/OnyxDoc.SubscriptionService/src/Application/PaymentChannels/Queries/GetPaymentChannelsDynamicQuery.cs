using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.SubscriptionService.Application.Common.Exceptions;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ReventInject;

namespace OnyxDoc.SubscriptionService.Application.PaymentChannels.Queries
{
    public class GetPaymentChannelsDynamicQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string SearchText { get; set; }
        public string UserId { get; set; }
    }

    public class GetPaymentChannelsDynamicQueryHandler : IRequestHandler<GetPaymentChannelsDynamicQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetPaymentChannelsDynamicQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetPaymentChannelsDynamicQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId,  request.UserId);
                var list = await _context.PaymentChannels.Where(a => a.SubscriberId == request.SubscriberId).ToListAsync();

                if (!string.IsNullOrEmpty(request.SearchText))
                {
                    list = list.Where(a => request.SearchText.IsIn(a.CurrencyCode.ToString()) 
                    || request.SearchText.IsIn(a.Name) 
                    || request.SearchText.IsIn(a.TransactionRateType.ToString())).ToList();
                }

                if (list == null)
                {
                    throw new NotFoundException(nameof(PaymentChannel));
                }
                var result = _mapper.Map<List<PaymentChannelDto>>(list);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving PaymentChannels. Error: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }

}
