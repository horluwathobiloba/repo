using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Application.Users.Command.CreateOTP;
using RubyReloaded.AuthService.Application.Users.Command.VerifyInvitationCode;
using RubyReloaded.AuthService.Application.Users.Queries.GetUser;
using RubyReloaded.AuthService.Domain.Entities;
using RubyReloaded.AuthService.Domain.Enums;
using RubyReloaded.AuthService.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.User.Command.CreateUser
{
    //User does not exist o the system before 
    public class CreateUserCooperativeCommand : IRequest<Result>
    {
        public string FirstName { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public int CooperativeId { get; set; }
        public string Code { get; set; }
    }

    public class CreateUserCooperativeCommandHandler : IRequestHandler<CreateUserCooperativeCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly IBase64ToFileConverter _fileConverter;
        private readonly IStringHashingService _stringHashingService;
        private readonly IConfiguration _configuration;
        public CreateUserCooperativeCommandHandler(IApplicationDbContext context, IIdentityService identityService, IMapper mapper,
            IBase64ToFileConverter fileConverter, IEmailService emailService, IStringHashingService stringHashingService,
            IConfiguration configuration)
        {
            _context = context;
            _identityService = identityService;
            _emailService = emailService;
            _mapper = mapper;
            _fileConverter = fileConverter;
            _stringHashingService = stringHashingService;
            _configuration = configuration;
        }
        public async Task<Result> Handle(CreateUserCooperativeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var createdBy = "";
                
                var user = await _identityService.GetUserByEmail(request.Email);
                if (user.user != null)
                {
                    return Result.Failure(new string[] { "User already exists with this email" });
                }

                var cooperative = await _context.Cooperatives.Include(x=>x.CooperativeSetting).FirstOrDefaultAsync(x => x.Id == request.CooperativeId);
                var cooperativeSettings = await _context.CooperativeSettings.FirstOrDefaultAsync(x => x.CooperativeId == cooperative.Id);
             
                if (cooperative.CooperativeType==CooperativeType.Private)
                {
                    var verificationCommand = new VerifyInvitationCodeCommand
                    {
                        Email = request.Email,
                        Code = request.Code,
                        CooperativeId = request.CooperativeId
                    };
                    var verificationHandler = await new VerifyInvitationCodeCommandHandler(_context).Handle(verificationCommand, cancellationToken);
                    if (!verificationHandler.Succeeded)
                    {
                        return Result.Failure("Verification Failed:" + verificationHandler.Message);
                    }
                }
            

                if (cooperativeSettings.RequestToJoin == false)
                {
                    await _context.BeginTransactionAsync();
                    var newUser2 = new Domain.Entities.User
                    {
                       
                        Email = request.Email.Trim(),
                        FirstName = request.FirstName.Trim(),
                        LastName = request.LastName.Trim(),
                        PhoneNumber = request.PhoneNumber,
                        UserName=request.UserName,
                        Password = request.Password.Trim(),
                        StatusDesc = Status.Active.ToString(),
                        Status = Status.Active,
                        CreatedDate = DateTime.Now,
                        CreatedBy = createdBy,
                      
                        //ProfilePicture = await _fileConverter.ConvertBase64StringToFile(request.ProfilePicture, request.FirstName + "_" + request.LastName + ".png"),
                    };

                    newUser2.Name = string.Concat(newUser2.FirstName, " ", newUser2.LastName);
                    var hashValue = (newUser2.Email + DateTime.Now).ToString();
                    newUser2.Token = _stringHashingService.CreateMD5StringHash(hashValue);
                    var result2 = await _identityService.CreateUserAsync(newUser2);
                    if (!result2.Result.Succeeded)
                    {
                        return Result.Failure(result2.Result.Messages);
                    }
                    await _context.SaveChangesAsync(cancellationToken);
                    var userCooperativeMapping2 = new CooperativeUserMapping
                    {
                        
                        Status = Status.Active,
                        Cooperative = cooperative,
                        CreatedDate = DateTime.Now,
                        CooperativeId = request.CooperativeId,
                        Email = request.Email,
                        UserId = result2.UserId,
                        Name = newUser2.Name,
                        StatusDesc = Status.Active.ToString(),
                        CooperativeAccessStatus=CooperativeAccessStatus.Approved
                    };
                    await _context.CooperativeMembers.AddAsync(userCooperativeMapping2);
                    await _context.SaveChangesAsync(cancellationToken);
                    var sendOtpRequest2 = new CreateOTPCommand
                    {
                        Email = request.Email,
                        Reason = "Email Verification"
                    };
                    var handler2 = await new CreateOTPCommandHandler(_context, _identityService, _emailService,_configuration).Handle(sendOtpRequest2, cancellationToken);
                    if (!handler2.Succeeded)
                    {
                        return Result.Failure("Otp Could not be sent:" + handler2.Message);
                    }
                    //TODO: Update email with login details, username,password,organizationcode
                    string webDomain = _configuration["WebDomain"];
                   
                    //var email2 = new EmailVm
                    //{
                    //    Application = "Ruby",
                    //    Subject = "Email Verification",
                    //    Text = "User created successfully",
                    //    RecipientEmail = request.Email,
                    //    RecipientName = request.FirstName,
                    //    DisplayButton = "User Created successfully",
                    //    Body1 = "Please see login details below ;",
                    //    Body2 = "Password: " + request.Password,
                    //    Otp = handler2.Entity.ToString()
                    //};
                    //// await _emailService.CooperativeSignUp(email);
                    //await _emailService.SendOtp(email2);
                    await _context.CommitTransactionAsync();
                    return Result.Success("User creation was successful", userCooperativeMapping2);
                }


                #region ForRequestToJoin

                var newUser = new Domain.Entities.User
                {
                    UserAccessLevel=UserAccessLevel.NormalUser,
                    Email = request.Email.Trim(),
                    FirstName = request.FirstName.Trim(),
                    LastName = request.LastName.Trim(),
                    Password = request.Password.Trim(),
                    StatusDesc = Status.Active.ToString(),
                    Status = Status.Active,
                    CreatedDate = DateTime.Now,
                    CreatedBy = createdBy,
                   
                };

                newUser.Name = string.Concat(newUser.FirstName, " ", newUser.LastName);

                var result = await _identityService.CreateUserAsync(newUser);
                if (!result.Result.Succeeded)
                {
                    return Result.Failure(result.Result.Messages);
                }
                await _context.SaveChangesAsync(cancellationToken);
                var userCooperativeMapping = new CooperativeUserMapping
                {
                    Status = Status.Active,
                    Cooperative = cooperative,
                    CreatedDate = DateTime.Now,
                    CooperativeId = request.CooperativeId,
                    Email = request.Email,
                    UserId = result.UserId,
                    Name = newUser.Name,
                    StatusDesc = Status.Active.ToString(),
                    CooperativeAccessStatus = CooperativeAccessStatus.Processing
                };
                await _context.CooperativeMembers.AddAsync(userCooperativeMapping);
                await _context.SaveChangesAsync(cancellationToken);
                var sendOtpRequest = new CreateOTPCommand
                {
                    Email = request.Email,
                    Reason = "Email Verification"
                };
                var handler = await new CreateOTPCommandHandler(_context, _identityService, _emailService,_configuration).Handle(sendOtpRequest, cancellationToken);
                if (!handler.Succeeded)
                {
                    return Result.Failure("Otp Could not be sent:" + handler.Message);
                }

                //var email = new EmailVm
                //{
                //    Application = "Ruby",
                //    Subject = "Email Verification",
                //    Text = "User created successfully",
                //    RecipientEmail = request.Email,
                //    RecipientName = request.FirstName,
                //    DisplayButton = "User Created successfully",
                //    Body1 = "Please see login details below ;",
                //    Body2 = "Password: " + request.Password,
                //    Otp = handler.Entity.ToString()
                //};
                //// await _emailService.CooperativeSignUp(email);
                //await _emailService.SendOtp(email);
                return Result.Success("User creation was successful and email for requesting access sent successfully", user);
                #endregion           
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "User creation was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
