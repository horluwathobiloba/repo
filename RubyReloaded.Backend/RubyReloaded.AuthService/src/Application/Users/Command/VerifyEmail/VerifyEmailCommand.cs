using AutoMapper;
using MediatR;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Application.Users.Command.VerifyOtp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.Users.Command.VerifyEmail
{
    public class VerifyEmailCommand:IRequest<Result>
    {
        public string Email { get; set; }
        public string OTP { get; set; }
    }

    public class GetVerifyEmailHandler : IRequestHandler<VerifyEmailCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;
        public GetVerifyEmailHandler(IApplicationDbContext context, IIdentityService identityService, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _identityService = identityService;
        }
        public async Task<Result> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var resultEmail = await _identityService.GetUserByEmail(request.Email);
                if (resultEmail.user == null)
                {
                    return Result.Failure("Invalid User");
                }
                var verifyOtpRequest = new VerifyOtpCommand
                {
                    Email = request.Email,
                    OTP = request.OTP
                };
                var handler = await new VerifyOtpCommandHandler(_context, _identityService).Handle(verifyOtpRequest, cancellationToken);
             
                if (!handler.Succeeded)
                {
                    return Result.Failure(handler.Message);
                }
                var confirmResult = await _identityService.VerifyEmailAsync(resultEmail.user);
                if (confirmResult.Succeeded)
                {
                    return Result.Success(confirmResult.Message);
                }
                return Result.Failure("Operation failed");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Error verifying customer", ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
