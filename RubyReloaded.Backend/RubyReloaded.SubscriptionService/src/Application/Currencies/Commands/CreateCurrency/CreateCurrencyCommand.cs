using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.SubscriptionService.Application.Common.Interfaces;
using RubyReloaded.SubscriptionService.Application.Common.Models;
using RubyReloaded.SubscriptionService.Application.Currencies.Queries;
using RubyReloaded.SubscriptionService.Domain.Entities;
using RubyReloaded.SubscriptionService.Domain.Enums;
using ReventInject;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.SubscriptionService.Application.Currencies.Commands
{
    public class CreateCurrencyCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public CurrencyCode CurrencyCode { get; set; }
        public bool IsDefault { get; set; }
        public string UserId { get; set; }
    }

    public class CreateCurrencyCommandHandler : IRequestHandler<CreateCurrencyCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreateCurrencyCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _authService = authService;
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(CreateCurrencyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);


                var exists = await _context.Currencies.AnyAsync(a => a.SubscriberId == request.SubscriberId && a.CurrencyCode == request.CurrencyCode);

                if (exists)
                {
                    return Result.Failure($"Currency code '{request.CurrencyCode}' already exists.");
                }

                var entity = new Currency
                {
                    Name = request.CurrencyCode.ToString(),
                    SubscriberId = request.SubscriberId,
                    SubscriberName = _authService.Subscriber?.Name,
                    CurrencyCode = request.CurrencyCode,
                    CurrencyCodeDesc = request.CurrencyCode.ToString(),
                    IsDefault = request.IsDefault,
                    UserId = request.UserId,
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = request.UserId,
                    LastModifiedDate = DateTime.Now,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString()
                };

                await _context.Currencies.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<CurrencyDto>(entity);
                return Result.Success("Currency created successfully!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Currency creation failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
