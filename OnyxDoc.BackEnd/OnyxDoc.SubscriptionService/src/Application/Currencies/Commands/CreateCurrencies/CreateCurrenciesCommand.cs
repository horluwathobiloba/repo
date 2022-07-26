using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Application.Currencies.Queries;
using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.Currencies.Commands
{
    public class CreateCurrenciesCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; } 
        public List<CreateCurrencyRequest> Currencies { get; set; }
        public string UserId { get; set; }
    }

    public class CreateCurrenciesCommandHandler : IRequestHandler<CreateCurrenciesCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreateCurrenciesCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(CreateCurrenciesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var list = new List<Currency>();

                await _context.BeginTransactionAsync();

                foreach (var item in request.Currencies)
                {
                    var exists = await _context.Currencies.AnyAsync(x => x.SubscriberId == request.SubscriberId && x.CurrencyCode  == item.CurrencyCode);
                    if (exists)
                    {
                        return Result.Failure($"Currency code '{item.CurrencyCode}' already exists!");
                    }
                    var entity = new Currency
                    {
                        Name = item.CurrencyCode.ToString(),
                        SubscriberId = request.SubscriberId,
                        SubscriberName = _authService.Subscriber?.SubscriberName,
                        CurrencyCode = item.CurrencyCode,
                        CurrencyCodeDesc = item.CurrencyCode.ToString(),
                        IsDefault = item.IsDefault,
                        CreatedBy = request.UserId,
                        CreatedDate = DateTime.Now,
                        LastModifiedBy = request.UserId,
                        LastModifiedDate = DateTime.Now,
                        Status = Status.Active,
                        StatusDesc = Status.Active.ToString()
                    };
                    list.Add(entity);
                }
                await _context.Currencies.AddRangeAsync(list);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                var result = _mapper.Map<List<CurrencyDto>>(list);
                return Result.Success("Currencies created successfully!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Currencies creation failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
