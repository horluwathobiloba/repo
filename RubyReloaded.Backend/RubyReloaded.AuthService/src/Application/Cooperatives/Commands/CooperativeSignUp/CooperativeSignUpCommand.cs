using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Application.Users.Command.CreateOTP;
using RubyReloaded.AuthService.Domain.Entities;
using RubyReloaded.AuthService.Domain.Enums;
using RubyReloaded.AuthService.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.Cooperatives.Commands.CooperativeSignUp
{
    public class CooperativeSignUpCommand : IRequest<Result>
    {
        #region Cooperative
        public string CoopName { get; set; }
        public string CoopEmail { get; set; }

        public string CoopPhoneNumber { get; set; }

        public string State { get; set; }
        public string Country { get; set; }

        #endregion
        #region Role
        // public int CooperativeId { get; set; }
        //public string UserId { get; set; }
   
        // public int RoleId { get; set; } 
        #endregion

        #region User

        public string FirstName { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; }
        public string UserPhoneNumber { get; set; }
        public string UserEmail { get; set; }





        #endregion

    }

    public class CooperativeSignUpCommandHandler : IRequestHandler<CooperativeSignUpCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IBase64ToFileConverter _fileConverter;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly ISqlService _sqlService;
        private readonly IStringHashingService _stringHashingService;
        public CooperativeSignUpCommandHandler(IApplicationDbContext context, IIdentityService identityService,
            IBase64ToFileConverter fileConverter, IEmailService emailService, IConfiguration configuration,
            ISqlService sqlService, IStringHashingService stringHashingService)
        {
            _context = context;
            _identityService = identityService;
            _fileConverter = fileConverter;
            _emailService = emailService;
            _configuration = configuration;
            _sqlService = sqlService;
            _stringHashingService = stringHashingService;
        }
        public async Task<Result> Handle(CooperativeSignUpCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var cooperativeExists = await _context.Cooperatives.AnyAsync(a => a.Name == request.CoopName);
                if (cooperativeExists)
                {
                    return Result.Failure(new string[] { "Cooperative Name already exist" });
                }
                var cooperative = new Cooperative
                {
                    CreatedDate = DateTime.Now,
                    StatusDesc = Status.Deactivated.ToString(),
                    Status = Status.Deactivated,
                    Email = request.CoopEmail,
                    PhoneNumber = request.CoopPhoneNumber,
                    Name = request.CoopName,
                    CooperativeType=CooperativeType.Public,
                    State=request.State,
                    Country=request.Country
                };

                await _context.BeginTransactionAsync();
                await _context.Cooperatives.AddAsync(cooperative);
                await _context.SaveChangesAsync(cancellationToken);
                var cooperativeId = cooperative.Id;
                var settings = new CooperativeSettings
                {
                    Name =request.CoopName,
                    AdminEmail=request.UserEmail,
                    Status=Status.Deactivated,
                    StatusDesc=Status.Deactivated.ToString(),
                    CooperativeId=cooperativeId,
                    Visible=false,
                    RequestToJoin=false,
                    
                };
                

                await _context.CooperativeSettings.AddAsync(settings);
                // we need to test this
                cooperative.CooperativeSetting = settings;
                cooperative.CooperativeSettingId = settings.Id;
                 _context.Cooperatives.Update(cooperative);
                await _context.SaveChangesAsync(cancellationToken);


                //CreatingUser
                var newUser = new Domain.Entities.User
                {
                    
                    Email = request.UserEmail.Trim(),
                    FirstName = request.FirstName.Trim(),
                    LastName = request.LastName.Trim(),
                    PhoneNumber = request.UserPhoneNumber,
                   
                    Password = request.Password.Trim(),
                    StatusDesc = Status.Active.ToString(),
                    Status = Status.Active,
                    CreatedDate = DateTime.Now,
                    CreatedBy = request.UserEmail,
                    Country = request.Country,
                    State = request.State,
                  
                };

                newUser.Name = string.Concat(newUser.FirstName, " ", newUser.LastName);
                var hashValue = (newUser.Email + DateTime.Now).ToString();
                newUser.Token = _stringHashingService.CreateMD5StringHash(hashValue);
                var result = await _identityService.CreateUserAsync(newUser);
                if (!result.Result.Succeeded)
                {
                    return Result.Failure(result.Result.Messages);
                }
                await _context.SaveChangesAsync(cancellationToken);
                //create the mapping of the user to the cooperative
                var userCooperativeMapping = new CooperativeUserMapping
                {
                    //RoleId = adminRole.Id,
                    Status = Status.Deactivated,
                    Cooperative = cooperative,
                    CreatedDate = DateTime.Now,
                    CooperativeId = cooperativeId,
                    Email = request.UserEmail,
                    UserId = result.UserId,
                    Name = newUser.Name,
                    StatusDesc = Status.Deactivated.ToString()
                };
                await _context.CooperativeMembers.AddAsync(userCooperativeMapping);
                await _context.SaveChangesAsync(cancellationToken);


                await _context.CommitTransactionAsync();
                var sendOtpRequest = new CreateOTPCommand
                {
                    Email = request.UserEmail,
                    Reason = "Email Verification"
                };
                //var handler = await new CreateOTPCommandHandler(_context, _identityService, _emailService).Handle(sendOtpRequest, cancellationToken);
                //if (!handler.Succeeded)
                //{
                //    return Result.Failure("OTP could not be sent" + handler.Message);
                //}
                
                string webDomain = _configuration["WebDomain"];
                var OTPresult = await _identityService.GenerateOTP(request.UserEmail, sendOtpRequest.Reason);
                if (!OTPresult.success)
                {
                    return Result.Failure("OTP generation failed");
                }
               

                var email = new EmailVm
                {
                    Application = "RubyReloaded",
                    Subject = "Email Verification",
                    BCC = "",
                    CC = "",
                    RecipientName = newUser.FirstName,
                    RecipientEmail = newUser.Email,
                    FirstName = newUser.FirstName,
                    LastName =newUser.LastName,
                    Body = $"Click On the link to verify your email",
                    //Body1= "Please see login details below ;",
                    //Body2
                    Otp = OTPresult.token,
                    ButtonText = "Click The Link To Verify Your Password",
                    ButtonLink = webDomain + $"verify?email={newUser.Email}&token={newUser.Token}'",
                    ImageSource = _configuration["SVG:EmailVerification"],
                };
                await _emailService.CooperativeSignUp(email);

                //when we figure out super Admin

                // var email2 = new EmailVm
                // {
                //     Application = "Ruby",
                //     Subject = "Email Verification",
                //     Text = "User created successfully",
                //     RecipientEmail = request.UserEmail,
                //     FirstName = request.FirstName,
                //     LastName = request.LastName,
                //     DisplayButton = "User Created successfully",
                //     Body1 = "Use The OTP for Account Verification;",
                //     Body2 = "Password: " + request.Password,
                //     ButtonLink = link,
                //     Otp = handler.Entity.ToString()
                // };
                //// await _emailService.SendOtp(email2);
                return Result.Success(userCooperativeMapping);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { " SignUp was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
