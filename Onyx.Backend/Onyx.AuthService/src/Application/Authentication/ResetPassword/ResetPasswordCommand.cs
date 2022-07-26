
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Onyx.AuthService.Application.Common.Interfaces;
using Onyx.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.AuthService.Application.Authentication.ResetPassword
{
    public class ResetPasswordCommand:IRequest<Result>
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
    }


    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthenticateService _authenticationService;
        private readonly IEmailService _emailService;
        private readonly IIdentityService _identityService;
        private readonly IConfiguration _configuration;

        public ResetPasswordCommandHandler(IApplicationDbContext context,
            IAuthenticateService authenticationService,
             IEmailService emailService, IIdentityService identityService, IConfiguration configuration)
        {
            _context = context;
            _authenticationService = authenticationService;
            _emailService = emailService;
            _identityService = identityService;
            _configuration = configuration;
        }

        public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //check if password reset was initiated
                if (request.Email is null)
                {
                    return Result.Failure("Invalid Email");
                }
                var userDetail = await _identityService.GetUserByEmail(request.Email);
                if (userDetail.staff == null)
                {

                    return Result.Failure("User cannot be found");
                }
                var passwordResetAttempt = await _context.PasswordResetAttempts.OrderByDescending(a => a.CreatedDate).FirstOrDefaultAsync(a=>a.Email == request.Email && a.PasswordResetStatus == Domain.Enums.PasswordResetStatus.Initiated);
                if (passwordResetAttempt == null)
                {
                    return Result.Failure(new string[] { "Invalid Password Reset Attempt"});
                }

                TimeSpan timeDifference = DateTime.Now - passwordResetAttempt.CreatedDate;
                if (timeDifference.TotalHours > 2 )
                {
                    return Result.Failure(new string[] { "Reset Password link has expired.Kindly reinitiate another password reset" });
                }
                var result = await _authenticationService.ResetPassword(request.Email, request.NewPassword);
                if (result.Succeeded)
                {

                    passwordResetAttempt.PasswordResetStatus = Domain.Enums.PasswordResetStatus.Completed;
                    passwordResetAttempt.PasswordResetStatusDesc = Domain.Enums.PasswordResetStatus.Completed.ToString();
                     _context.PasswordResetAttempts.Update(passwordResetAttempt);
                   await  _context.SaveChangesAsync(cancellationToken);
                  
                    string webDomain = _configuration["WebDomain"];
                    var email = new EmailVm
                    { 
                            Application = "Onyx",
                            Subject = "Password Reset Successful",
                            RecipientEmail = request.Email,
                            FirstName = userDetail.staff.FirstName,
                            LastName = userDetail.staff.LastName,
                            Body = "Your password has been reset.You can now login to continue your activities.",
                            ButtonText = "Login",
                            ButtonLink = webDomain + $"login?email={request.Email}&token={userDetail.staff.Token}'",
                            ImageSource = _configuration["SVG:EmailVerification"],
                    };

                    await _emailService.ResetPasswordEmailAsync(email);

                    return Result.Success("Password Reset Successful");
                }
                return Result.Failure("Reset Password failed");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Error changing password!", ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
