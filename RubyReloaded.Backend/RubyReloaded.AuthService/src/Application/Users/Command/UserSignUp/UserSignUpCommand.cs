using MediatR;
using Microsoft.Extensions.Configuration;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Application.Users.Command.CreateOTP;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.Users.Command.UserSignUp
{
    public class UserSignUpCommand : IRequest<Result>
    {
        public string FirstName { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

    }

    public class UserSignUpCommanddHandler : IRequestHandler<UserSignUpCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IStringHashingService _stringHashingService;
        public UserSignUpCommanddHandler(IApplicationDbContext context, IIdentityService identityService,
            IBase64ToFileConverter fileConverter, IEmailService emailService,
            IConfiguration configuration, IStringHashingService stringHashingService)
        {
            _context = context;
            _identityService = identityService;
            _emailService = emailService;
            _configuration = configuration;
            _stringHashingService = stringHashingService;
        }
        public async Task<Result> Handle(UserSignUpCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _identityService.GetUserByEmail(request.Email);
                if (user.user != null)
                {
                    return Result.Failure(new string[] { "User already exists with this email" });
                }
                var newUser = new Domain.Entities.User
                {
                    Email = request.Email.Trim(),
                    FirstName = request.FirstName.Trim(),
                    LastName = request.LastName.Trim(),
                    PhoneNumber = request.PhoneNumber,
                    Password = request.Password.Trim(),
                    StatusDesc = Status.Active.ToString(),
                    Status = Status.Active,
                    CreatedDate = DateTime.Now,
                    UserName = request.UserName
                };

                newUser.Name = string.Concat(newUser.FirstName, " ", newUser.LastName);
                var hashValue = (newUser.Email + DateTime.Now).ToString();
                newUser.Token = _stringHashingService.CreateMD5StringHash(hashValue);
                var result = await _identityService.CreateUserAsync(newUser);
                if (!result.Result.Succeeded)
                {
                    return Result.Failure(result.Result.Message ?? result.Result?.Messages[0]);
                }
                var sendOtpRequest = new CreateOTPCommand
                {
                    Email = request.Email,
                    Reason = "Email Verification"
                };
                var handler = await new CreateOTPCommandHandler(_context, _identityService, _emailService, _configuration).Handle(sendOtpRequest, cancellationToken);
                if (!handler.Succeeded)
                {
                    return Result.Failure("OTP Initiation failed:" + handler.Message);
                }
                return Result.Success("User creation was successful", newUser);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "User status change was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    } 
}
    
