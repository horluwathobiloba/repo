
using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Domain.Entities;
using RubyReloaded.WalletService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Currencys.Commands.CreateCurrencies
{
    public class CreateCurrenciesCommand : IRequest<Result>
    {
        public string[] CurrencyCode { get; set; }
        public string LoggedInUserId { get; set; }
    }

    public class CreateCurrenciesCommandHandler : IRequestHandler<CreateCurrenciesCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public CreateCurrenciesCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(CreateCurrenciesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var getCurrencies = await _context.Currencies.ToListAsync();
                var currencies = new List<Domain.Entities.Currency>();
                var wrongCurrencies = new List<string>();

                foreach (var currency in request.CurrencyCode)
                {
                    if (!getCurrencies.Any(x=>x.CurrencyCodeString==currency))
                    {
                        currencies.Add(new Currency
                        {
                            CurrencyCode = (CurrencyCode)Enum.Parse(typeof(CurrencyCode), currency),
                            CurrencyCodeString = currency.ToString(),
                            CreatedBy = request.LoggedInUserId,
                            CreatedDate = DateTime.Now,
                            Status = Status.Active
                        });
                    }
                    else
                    {
                        wrongCurrencies.Add(currency);
                    }
                }
                await _context.Currencies.AddRangeAsync(currencies);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Currencies  was created successfully", new { currencies, wrongCurrencies });
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Currencies creation was not successful", ex?.Message ?? ex?.InnerException.Message });
            }

        }
    }
}
