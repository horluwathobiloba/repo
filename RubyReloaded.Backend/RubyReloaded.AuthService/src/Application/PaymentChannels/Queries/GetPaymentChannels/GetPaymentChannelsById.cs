using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Exceptions;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.Entities;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.PaymentChannels.Queries.GetPaymentChannels
{
    public class GetPaymentChannelByIdQuery : IRequest<Result>
    {
        public int Id { get; set; }
    }
    public class GetPaymentChannelByIdQueryHandler : IRequestHandler<GetPaymentChannelByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetPaymentChannelByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetPaymentChannelByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.PaymentChannels.FirstOrDefaultAsync(a => a.Id == request.Id && a.Status == Status.Active);

                if (entity == null)
                {
                    throw new NotFoundException(nameof(PaymentChannel), request.Id);
                }

                if (entity.Status == Status.Deactivated)
                {
                    throw new NotFoundException($"PaymentChannel id={request.Id} does not exist. Please contact support.");
                }
               // var paymentChannel = _mapper.Map<PaymentChannelDto>(entity);
                return Result.Success(entity);
            }
            catch (Exception ex)
            {
                
                return Result.Failure(ex?.Message ?? ex?.InnerException?.Message);
            }

        }
    }
}
