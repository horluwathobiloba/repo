using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Application.Users.Command.CreateOTP;
using RubyReloaded.AuthService.Domain.Entities;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.Ajos.Command.AjoSignUp
{
    public class CreateAjoUserCommand:IRequest<Result>
    {
        public string FirstName { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Code { get; set; }
        public int AjoId { get; set; }
       
       
    }
    public class CreateAjoUserCommandHandler : IRequestHandler<CreateAjoUserCommand, Result>
    {

        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly IBase64ToFileConverter _fileConverter;
        private readonly IStringHashingService _stringHashingService;
        private readonly IConfiguration _configuration;
        public CreateAjoUserCommandHandler(IApplicationDbContext context, IIdentityService identityService, IMapper mapper,
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
        public async Task<Result> Handle(CreateAjoUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var createdBy = "";
                var user = await _identityService.GetUserByEmail(request.Email);
                if (user.user != null)
                {
                    return Result.Failure(new string[] { "User already exists with this email" });
                }
                var ajo = await _context.Ajos.FirstOrDefaultAsync(x => x.Id == request.AjoId);

                // validate code for joining ajo
                var newUser2 = new Domain.Entities.User
                {
                    UserAccessLevel = UserAccessLevel.NormalUser,
                    Email = request.Email.Trim(),
                    FirstName = request.FirstName.Trim(),
                    LastName = request.LastName.Trim(),
                    PhoneNumber = request.PhoneNumber,
                  //UserId = request.UserId,
                    Password = request.Password.Trim(),
                    StatusDesc = Status.Active.ToString(),
                    Status = Status.Active,
                    CreatedDate = DateTime.Now,
                    CreatedBy = createdBy,
                    UserName=request.UserName
                    //Country = request.Country,
                    //State = request.State,
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
                var userCooperativeMapping2 = new AjoMember
                {
                    RoleId = 0,
                    Status = Status.Active,
                    Ajo = ajo,
                    CreatedDate = DateTime.Now,
                    AjoId = request.AjoId,
                    Email = request.Email,
                    UserId = result2.UserId,
                    Name = newUser2.Name,
                    StatusDesc = Status.Active.ToString()
                };
                await _context.AjoMembers.AddAsync(userCooperativeMapping2);
                await _context.SaveChangesAsync(cancellationToken);

                var sendOtpRequest = new CreateOTPCommand
                {
                    Email = request.Email,
                    Reason = "Email Verification"
                };
                var handler = await new CreateOTPCommandHandler(_context, _identityService, _emailService,_configuration).Handle(sendOtpRequest, cancellationToken);
                if (!handler.Succeeded)
                {
                    return Result.Failure("OTP could not be sent" + handler.Message);
                }
                return Result.Success(" User creation was successful", userCooperativeMapping2);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "User creation was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
