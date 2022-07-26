using MediatR;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.Users.Command.VerifyUserTransactionPin
{
    public class VerifyTransactionPinCommand:IRequest<Result>
    {
        public string UserId { get; set; }
        public string Pin { get; set; }
    }

    public class VerifyTransactionPinCommandHandler : IRequestHandler<VerifyTransactionPinCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        public VerifyTransactionPinCommandHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }
        public async Task<Result> Handle(VerifyTransactionPinCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _identityService.VerifyTransactionPin(request.UserId, request.Pin);
                return result;
            }
            catch (Exception ex)
            {
                return Result.Failure("");
            }
        }
    }
}
