using MediatR;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.CurrencyConfigurations.Commands.ChangeCurrencyConfigurations
{
    public class ChangeCurrencyConfigurationStatusCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public string LoggedInUserId { get; set; }
    }

    public class ChangeCurrencyConfigurationStatusCommandHandler : IRequestHandler<ChangeCurrencyConfigurationStatusCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public ChangeCurrencyConfigurationStatusCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(ChangeCurrencyConfigurationStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string message = "";
                var configuration = await _context.CurrencyConfigurations.FindAsync(request.Id);
                if (configuration == null)
                {
                    return Result.Failure(new string[] { "Currency Configuration is invalid" });
                }
                switch (configuration.Status)
                {
                    case Domain.Enums.Status.Active:
                        configuration.Status = Domain.Enums.Status.Inactive;
                        message = "Currency Configuration status was changed to inactive successfully";
                        break;
                    case Domain.Enums.Status.Inactive:
                        configuration.Status = Domain.Enums.Status.Active;
                        message = "Currency Configuration status was changed to active successfully";
                        break;
                    default:
                        break;
                }
                _context.CurrencyConfigurations.Update(configuration);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success(message);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Currency Configuration deactivation was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
