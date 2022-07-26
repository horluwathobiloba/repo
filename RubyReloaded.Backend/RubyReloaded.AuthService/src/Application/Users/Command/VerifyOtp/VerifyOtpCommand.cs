using MediatR;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.Users.Command.VerifyOtp
{
    public class VerifyOtpCommand:IRequest<Result>
    {
        public string Email { get; set; }
        public string OTP { get; set; }
    }



    public class VerifyOtpCommandHandler : IRequestHandler<VerifyOtpCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
    
        public VerifyOtpCommandHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
          
        }

        public async Task<Result> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.ValidateOTP(request.OTP, request.Email);
            if (result.success)
            {
                return Result.Success("Verified",result);
            }
            return Result.Failure("OTP is invalid");
        }
    }
}
