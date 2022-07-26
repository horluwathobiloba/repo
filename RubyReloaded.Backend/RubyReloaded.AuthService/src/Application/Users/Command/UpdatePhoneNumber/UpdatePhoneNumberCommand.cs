using MediatR;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.Users.Command.UpdatePhoneNumber
{
    public class UpdatePhoneNumberCommand:IRequest<Result>
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class UpdatePhoneNumberCommandHandler : IRequestHandler<UpdatePhoneNumberCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        public UpdatePhoneNumberCommandHandler(IApplicationDbContext context,IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }
        public async Task<Result> Handle(UpdatePhoneNumberCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _identityService.ChangeUserPhoneNumber(request.Email, request.PhoneNumber);
                if (!result.Succeeded)
                {
                    return Result.Failure(result.Message);
                }
                return result;
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "User update was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
