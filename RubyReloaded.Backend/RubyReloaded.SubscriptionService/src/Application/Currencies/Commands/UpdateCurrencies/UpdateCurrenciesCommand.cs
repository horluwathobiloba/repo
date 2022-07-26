using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.SubscriptionService.Application.Common.Models;
using RubyReloaded.SubscriptionService.Application.Common.Interfaces;
using RubyReloaded.SubscriptionService.Domain.Enums;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RubyReloaded.SubscriptionService.Domain.Entities;
using RubyReloaded.SubscriptionService.Application.Currencies.Queries;

namespace RubyReloaded.SubscriptionService.Application.Currencies.Commands
{
    public class UpdateCurrenciesCommand : AuthToken, IRequest<Result>
    {
        public List<UpdateCurrencyRequest> Currencies { get; set; }
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public string UserId { get; set; }
    }

    public class UpdateCurrenciesCommandHandler : IRequestHandler<UpdateCurrenciesCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;


        public UpdateCurrenciesCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(UpdateCurrenciesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId , request.UserId);
                var list = new List<Currency>();
                await _context.BeginTransactionAsync();

                foreach (var item in request.Currencies)
                {
                    //check if the name of the vendor type already exists and conflicts with this new name 
                    var UpdatedEntityExists = await _context.Currencies
                        .AnyAsync(x => x.SubscriberId == request.SubscriberId && x.Id != item.Id && x.CurrencyCode == item.CurrencyCode);

                    if (UpdatedEntityExists)
                    {
                        return Result.Failure($"Another currency named {item.CurrencyCode.ToString()} already exists for this payment channel. Please change the name and try again.");
                    }
                    var entity = await _context.Currencies.Where(x => x.SubscriberId == request.SubscriberId && x.Id == item.Id)
                                           .FirstOrDefaultAsync();

                    if (entity == null)
                    {
                        entity = new Currency
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
                    }
                    else
                    {
                        entity.Name = item.CurrencyCode.ToString();
                        entity.CurrencyCode = item.CurrencyCode;
                        entity.CurrencyCodeDesc = item.CurrencyCode.ToString();
                        entity.IsDefault = item.IsDefault;
                        entity.LastModifiedBy = request.UserId;
                        entity.LastModifiedDate = DateTime.Now;
                    }
                    list.Add(entity);
                }

                _context.Currencies.UpdateRange(list);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                var result = _mapper.Map<List<CurrencyDto>>(list);
                return Result.Success("Currencies update was successful!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Currency update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }


    }


}
