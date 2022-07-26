using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Cards.Commands.GetCardAuthorization
{
    public class GetCardAuthorizationByUserIdQuery : IRequest<Result>
    {
        public string UserId { get; set; }
    }

    public class GetCardAuthorizationByUserIdQueryHandler : IRequestHandler<GetCardAuthorizationByUserIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetCardAuthorizationByUserIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;

        }
        public async Task<Result> Handle(GetCardAuthorizationByUserIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var cardAuthorization = await _context.CardAuthorizations.Where(x => x.UserId == request.UserId 
                 && !string.IsNullOrWhiteSpace(x.AuthorizationCode) && x.Status == Domain.Enums.Status.Active).ToListAsync();
                if (cardAuthorization == null)
                {
                    return Result.Failure("Invalid Card Authorization Code");
                }
                return Result.Success(cardAuthorization);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Card Authorization was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }


}
