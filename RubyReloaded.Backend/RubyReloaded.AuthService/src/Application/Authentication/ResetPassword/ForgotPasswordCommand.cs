
using MediatR;
using Microsoft.Extensions.Configuration;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Application.Users.Command.CreateOTP;
using RubyReloaded.AuthService.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.Authentication
{
    public class ForgotPasswordCommand : IRequest<Result>
    {
        public string Email { get; set; }
    }

    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, Result>
    {
        private readonly IEmailService _emailservice;
        private readonly IAuthenticateService _authenticationService;
        private readonly IIdentityService _identityService;
        private readonly IConfiguration _configuration;
        private readonly IApplicationDbContext _context;
        private readonly IStringHashingService _stringHashingService;
        public ForgotPasswordCommandHandler(IEmailService emailService,
            IAuthenticateService authenticationService,
            IConfiguration configuration,
            IIdentityService identityService,
            IApplicationDbContext context, IStringHashingService stringHashingService)
        {
            _emailservice = emailService;
            _authenticationService = authenticationService;
            _identityService = identityService;
            _configuration = configuration;
            _context = context;
            _stringHashingService = stringHashingService;
        }
        public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Email is null)
                {
                    return Result.Failure("Invalid Email");
                }
                var userDetail = await _identityService.GetUserByEmail(request.Email);
                if (userDetail.user == null)
                {

                    return Result.Failure("User cannot be found");
                }
                //send Otp
                var sendOtpRequest = new CreateOTPCommand
                {
                    Email = request.Email,
                    Reason = "Forgot Password"
                };
                string webDomain = _configuration["WebDomain"];
                var result = await _identityService.GenerateOTP(request.Email, sendOtpRequest.Reason);
                if (!result.success)
                {
                    return Result.Failure("Reset Password Initiation failed:");
                }
                var email = new EmailVm
                {
                    Application = "RubyReloaded",
                    Subject = "Forgot Password",
                    BCC = "",
                    CC = "",
                    RecipientName = userDetail.user.Name,
                    RecipientEmail = userDetail.user.Email,
                    FirstName = userDetail.user.FirstName,
                    LastName = userDetail.user.LastName,
                    Body = $"You requested a forgot password for this email address {userDetail.user.Email}. We’ve got you covered.",
                    Otp = result.token,
                    ButtonText = "Click The Link To Reset Your Password",
                    ButtonLink = webDomain + $"reset?email={userDetail.user.Email}&token={userDetail.user.Token}'",
                    ImageSource = _configuration["SVG:EmailVerification"],
                };

                await _emailservice.SendForgotPasswordEmailAsync(email);

               
                var passwordAttempts = new PasswordResetAttempt
                {
                    Email = request.Email,
                    PasswordResetStatus = Domain.Enums.PasswordResetStatus.Initiated,
                    CreatedBy = request.Email,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = request.Email,
                    LastModifiedDate = DateTime.Now,
                    Status = Domain.Enums.Status.Active,
                    PasswordResetStatusDesc = Domain.Enums.PasswordResetStatus.Initiated.ToString()
                };
                await _context.PasswordResetAttempts.AddAsync(passwordAttempts);
                var hashValue = (userDetail.user.Email + DateTime.Now).ToString();
                var hashedValue = _stringHashingService.CreateMD5StringHash(hashValue);
                //customerDetail.User.Token = emailhash;
                userDetail.user.Token = hashedValue;

                await _identityService.UpdateUserAsync(userDetail.user);
                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success("Reset Password Initiated successfully");

            }
            catch (Exception ex)
            {
                return Result.Failure( "Reset Password Initiation failed:"+ ex?.Message??ex?.InnerException?.Message );
            }

        }
    }
}
