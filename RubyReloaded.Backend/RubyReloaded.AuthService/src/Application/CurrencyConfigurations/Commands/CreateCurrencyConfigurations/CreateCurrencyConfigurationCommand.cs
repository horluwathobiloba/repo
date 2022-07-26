using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.Entities;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.CurrencyConfigurations.Commands.CreateCurrencyConfiguration
{
    public class CreateCurrencyConfigurationCommand : IRequest<Result>
    {
        public string[] CurrencyCode { get; set; }
        public string LoggedInUserId { get; set; }
    }

    public class CreateCurrencyConfigurationCommandHandler : IRequestHandler<CreateCurrencyConfigurationCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public CreateCurrencyConfigurationCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(CreateCurrencyConfigurationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var getCurrencies = await _context.CurrencyConfigurations.ToListAsync();
                var configurations = new List<Domain.Entities.CurrencyConfiguration>();
                var wrongCurrencies = new List<string>();

                foreach (var currency in request.CurrencyCode)
                {
                    if (!getCurrencies.Any(x=>x.CurrencyCodeString==currency))
                    {
                        CurrencyConfiguration configuration = new CurrencyConfiguration
                        {
                            CurrencyCode = (CurrencyCode)Enum.Parse(typeof(CurrencyCode), currency),
                            CurrencyCodeString = currency.ToString(),
                            CreatedBy = request.LoggedInUserId,
                            CreatedDate = DateTime.Now,
                            Status = Status.Active
                        };
                        configurations.Add(configuration);
                    }
                    else
                    {
                        wrongCurrencies.Add(currency);
                    }
                   
                }
                

                await _context.CurrencyConfigurations.AddRangeAsync(configurations);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Currency Configuration was created successfully", new { configurations, wrongCurrencies });
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Currency Configuration creation was not successful", ex?.Message ?? ex?.InnerException.Message });
            }

        }
    }
}
