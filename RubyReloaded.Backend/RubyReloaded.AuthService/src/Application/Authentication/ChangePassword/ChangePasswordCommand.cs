using RubyReloaded.AuthService.Application.Common.Exceptions;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Domain.Entities;
using RubyReloaded.AuthService.Domain.Enums;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using RubyReloaded.AuthService.Application.Common.Models;
using System;
using Microsoft.Extensions.Configuration;

namespace RubyReloaded.AuthService.Application.Authentication.Commands.ChangePassword
{
    public partial class ChangePasswordCommand :  IRequest<Result>
    {
        public string Username { get; set; }
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
    }

    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        private readonly IAuthenticateService _authenticationService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IIdentityService _identityService;

        public ChangePasswordCommandHandler(IApplicationDbContext context, 
            IAuthenticateService authenticationService, IEmailService emailService,
            IConfiguration configuration, IIdentityService identityService)
        {
            _context = context;
            _authenticationService = authenticationService;
            _emailService = emailService;
            _configuration = configuration;
            _identityService = identityService;
        }

        public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.OldPassword == request.NewPassword)
                {
                    return Result.Failure(new string[] { "Error!, Old Password is same as New Password" });
                }
                var staff = await _identityService.GetUserByUsername(request.Username);
                string webDomain = _configuration["WebDomain"];
                var email = new EmailVm
                {
                    Application = "RubyReloaded",
                    Subject = "Change Password",
                    BCC = "",
                    CC = "",
                    Text = "",
                    RecipientEmail = request.Username,
                    NewPassword = request.NewPassword,
                    OldPassword = request.OldPassword,
                    Body = "Your password change was successful!",
                    Body1 = "Please click the button below to login",
                    ButtonText = "Login",
                    ButtonLink = webDomain + $"login"



                    //old implementation

                    //Application = "RubyReloaded",
                    //Subject = "Change Password",
                    //Text = "Your have requested for a password change, please use the link below to reset your password.",
                    //RecipientEmail = request.Username,
                    //Password = request.OldPassword,
                    //ButtonText = "Click here to reset your password",
                    //Body = ""
                };
                await _emailService.CooperativeSignUp(email);
                return await _authenticationService.ChangePassword(request.Username, request.OldPassword, request.NewPassword);
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { " Error changing password : " + ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
