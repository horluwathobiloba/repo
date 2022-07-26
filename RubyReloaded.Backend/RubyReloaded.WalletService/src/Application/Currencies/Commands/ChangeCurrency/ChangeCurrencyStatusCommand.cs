
using MediatR;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Currencys.Commands.ChangeCurrency
{
    public class ChangeCurrencyStatusCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public string LoggedInUserId { get; set; }
    }

    public class ChangeCurrencyStatusCommandHandler : IRequestHandler<ChangeCurrencyStatusCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public ChangeCurrencyStatusCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(ChangeCurrencyStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string message = "";
                var currency = await _context.Currencies.FindAsync(request.Id);
                if (currency == null)
                {
                    return Result.Failure(new string[] { "Currency  is invalid" });
                }
                switch (currency.Status)
                {
                    case Domain.Enums.Status.Active:
                        currency.Status = Domain.Enums.Status.Inactive;
                        message = "Currency  status was changed to inactive successfully";
                        break;
                    case Domain.Enums.Status.Inactive:
                        currency.Status = Domain.Enums.Status.Active;
                        message = "Currency  status was changed to active successfully";
                        break;
                    default:
                        break;
                }
                _context.Currencies.Update(currency);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success(message);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Curreny status change was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }

}
