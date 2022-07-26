using MediatR;
using Microsoft.Extensions.Configuration;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.Users.Command.CreateOTP
{
    public class CreateOTPCommand:IRequest<Result>
    {
        public string Email { get; set; }
        public string Reason { get; set; }
    }

    public class CreateOTPCommandHandler : IRequestHandler<CreateOTPCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        public CreateOTPCommandHandler(IApplicationDbContext context, IIdentityService identityService, IEmailService emailService,IConfiguration configuration)
        {
            _context = context;
            _identityService = identityService;
            _emailService = emailService;
            _configuration = configuration;
        }

        public async Task<Result> Handle(CreateOTPCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.GenerateOTP(request.Email,request.Reason);
            string webDomain = _configuration["WebDomain"];
            var link = webDomain + $"verify?email={request.Email}&token={result.token}'";
            if (result.success)
            {
                var email2 = new EmailVm
                {
                    Application = "Ruby",
                    Subject = "OTP Validation",
                    Text = request.Reason,
                    RecipientEmail = request.Email,
                    Body=request.Reason,
                    Otp = result.token,
                    ButtonLink=link,
                    ButtonText="Click on the link to verify"
                };
                await _emailService.SendOtp(email2);
                return Result.Success("Otp generated successfully",result.token);

            }
            return Result.Failure("Failed To generate OTP");
        }
    }
}
