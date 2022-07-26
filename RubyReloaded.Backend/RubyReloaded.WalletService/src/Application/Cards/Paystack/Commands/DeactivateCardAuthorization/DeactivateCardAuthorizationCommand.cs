using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Domain.Enums;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Cards.Commands.DeactivateCardAuthorization
{
    public class DeactivateCardAuthorizationCommand : IRequest<Result>
    {
        public string UserId { get; set; }
        public string AuthorizationCode { get; set; }
    }

    public class DeactivateCardAuthorizationCommandHandler : IRequestHandler<DeactivateCardAuthorizationCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPaystackService _paystackService;
        public DeactivateCardAuthorizationCommandHandler(IApplicationDbContext context, IPaystackService paystackService)
        {
            _context = context;
            _paystackService = paystackService;
        }
        public async Task<Result> Handle(DeactivateCardAuthorizationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var cardAuthorization = await _context.CardAuthorizations.Where(a=>a.AuthorizationCode == request.AuthorizationCode && a.UserId == request.UserId).FirstOrDefaultAsync();
                if (cardAuthorization == null)
                {
                    return Result.Failure("Invalid Card Authorization Code");
                }
                var deactivationResponse = await _paystackService.DeactivateAuthorization(cardAuthorization.AuthorizationCode);
                if (!deactivationResponse.status)
                {
                    return Result.Failure("Deactivate card authorization on Paystack with code " + cardAuthorization.AuthorizationCode + "is " + deactivationResponse.status + " with message " + deactivationResponse.message);
                }
                cardAuthorization.Status = Status.Deactivated;
                cardAuthorization.StatusDesc = Status.Deactivated.ToString();

                 _context.CardAuthorizations.Update(cardAuthorization);
                var res = await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();
                return Result.Success("Card Authorization deactivated successfully");
            }
            catch (Exception ex)
            {
                _context.RollbackTransaction();
                return Result.Failure(new string[] { "Card Authorization deactivation was not successful", ex?.Message ?? ex?.InnerException.Message });
            }

        }
    }
}
