

using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Exceptions;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Domain.Entities;
using RubyReloaded.WalletService.Domain.Enums;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.PaymentChannels.Queries.GetPaymentChannels
{
    public class GetPaymentChannelsQuery : IRequest<Result>
    {
        public int Skip { get; set; }
        public int Take { get; set; }
    }

    public class GetPaymentChannelsQueryHandler : IRequestHandler<GetPaymentChannelsQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetPaymentChannelsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetPaymentChannelsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var entities = await _context.PaymentChannels.Where(a => a.Status != Status.Deactivated)
                    .Skip(request.Skip)
                    .Take(request.Take)
                      .ToListAsync();

                if (entities == null && entities.Count() <= 0)
                {
                    throw new NotFoundException(nameof(PaymentChannel), request);
                }
               // var paymentChannels = _mapper.Map<List<PaymentChannelListDto>>(entities);
                return Result.Success(entities);

                //return Result.Success("Success", resultList);
            }
            catch (Exception ex)
            {
                //throw new NotFoundException(nameof(PaymentChannel), request);
                return Result.Failure(ex?.Message ?? ex?.InnerException?.Message);
            }

        }
    }
}
