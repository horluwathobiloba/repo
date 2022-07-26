using MediatR;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.Users.Command.UpdateTransactionPin
{
    public class UpdateTransactionPinCommand:IRequest<Result>
    {
        public string Pin { get; set; }
        public string UserId { get; set; }
        public string LoggedInUserId { get; set; }
    }

    public class UpdateTransactionPinCommandHandler : IRequestHandler<UpdateTransactionPinCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        public UpdateTransactionPinCommandHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }
        public async Task<Result> Handle(UpdateTransactionPinCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.ChangeTransactionPin(request.UserId, request.Pin);
            if (result.Succeeded)
            {
                return Result.Success(result.Entity);
            }
            return Result.Failure(result.Message);
        }
    }
}
