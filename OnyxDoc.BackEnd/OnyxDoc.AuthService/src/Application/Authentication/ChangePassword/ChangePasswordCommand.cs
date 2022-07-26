using OnyxDoc.AuthService.Application.Common.Exceptions;
using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Domain.Entities;
using OnyxDoc.AuthService.Domain.Enums;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using OnyxDoc.AuthService.Application.Common.Models;
using System;
using Microsoft.Extensions.Configuration;

namespace OnyxDoc.AuthService.Application.Authentication.Commands.ChangePassword
{
    public partial class ChangePasswordCommand :  IRequest<Result>
    {
        public string Email { get; set; }
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
                var user = await _identityService.GetUserByEmail(request.Email);
                string webDomain = _configuration["WebDomain"];
                var email = new EmailVm
                {
                    Application = "OnyxDoc",
                    Subject = "Change Password",
                    BCC = "",
                    CC = "",
                    Text = "",
                    RecipientEmail = user.user.Email,
                    RecipientName=user.user.FirstName,
                    FirstName=user.user.FirstName,
                    LastName=user.user.LastName,
                    NewPassword = request.NewPassword,
                    OldPassword = request.OldPassword,
                    Body = "Your password change was successful!",
                    Body1 = "Please click the button below to login",
                    ButtonText = "Login",
                    ButtonLink = webDomain + $"login"

                };
                await _emailService.ChangePasswordEmail(email);
                return await _authenticationService.ChangePassword(request.Email, request.OldPassword, request.NewPassword);
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { " Error changing password : " + ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
