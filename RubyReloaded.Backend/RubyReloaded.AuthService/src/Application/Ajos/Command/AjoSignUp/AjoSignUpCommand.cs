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

    public class AjoSignUpCommand:IRequest<Result>
    {
        #region Ajo
        public CollectionCycle CollectionCycle { get; set; }
        public decimal CollectionAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberOfUsers { get; set; }
        public decimal AmountPerUser { get; set; }
        public decimal AmountToDisbursePerUser { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        #endregion

        #region Role
        // public int AjoId { get; set; }
        //public string UserId { get; set; }
        public string RoleName { get; set; }
        public string CreatedBy { get; set; }
        public AccessLevel AccessLevel { get; set; }
        // public int RoleId { get; set; } 
        #endregion



        #region User
        public string FirstName { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string UserId { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public int AjoId { get; set; }
        public int RoleId { get; set; }
        public string ProfilePicture { get; set; }
        #endregion
    }


    public class AjoSignUpCommandHandler : IRequestHandler<AjoSignUpCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IBase64ToFileConverter _fileConverter;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly ISqlService _sqlService;
        private readonly IStringHashingService _stringHashingService;
       
        public AjoSignUpCommandHandler(IApplicationDbContext context, IIdentityService identityService,
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
        public async Task<Result> Handle(AjoSignUpCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var AjoExists = await _context.Ajos.AnyAsync(a => a.Name == request.Name
                      && (a.Email == request.Email));
                if (AjoExists)
                {
                    return Result.Failure(new string[] { "Ajo details already exist" });
                }
                var ajo = new Ajo
                {
                    StartDate = request.StartDate,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString(),
                    Name = request.Name,
                    EndDate = request.EndDate,
                    CreatedDate = DateTime.Now,
                    AmountToDisbursePerUser = request.AmountToDisbursePerUser,
                    AmountPerUser = request.AmountPerUser,
                    CollectionAmount = request.CollectionAmount,
                    CollectionCycle = request.CollectionCycle,
                    NumberOfUsers = request.NumberOfUsers,
                    Code = request.Code
                };

                await _context.BeginTransactionAsync();
                await _context.Ajos.AddAsync(ajo);
                await _context.SaveChangesAsync(cancellationToken);

                //Untill we figure out roles and Permissions
               
                //CreatingUser
                var newUser = new Domain.Entities.User
                {
                    UserAccessLevel=UserAccessLevel.NormalUser,
                    Address = request.Address.Trim(),
                    Email = request.Email.Trim(),
                    FirstName = request.FirstName.Trim(),
                    LastName = request.LastName.Trim(),
                    PhoneNumber = request.PhoneNumber,
                    UserId = request.UserId,
                    Password = request.Password.Trim(),
                    StatusDesc = Status.Active.ToString(),
                    Status = Status.Active,
                    CreatedDate = DateTime.Now,
                    CreatedBy = request.Email,
                    Country = request.Country,
                    State = request.State,
                    ProfilePicture = await _fileConverter.ConvertBase64StringToFile(request.ProfilePicture, request.FirstName + "_" + request.LastName + ".png"),
                    UserName=request.UserName
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
                var userAjoMapping = new AjoMember
                {
                   CreatedBy=result.UserId,
                    Status = Status.Active,
                    Ajo = ajo,
                    CreatedDate = DateTime.Now,
                    AjoId = ajo.Id,
                    Email = request.Email,
                    UserId = result.UserId,
                    Name = newUser.Name,
                    StatusDesc = Status.Active.ToString()
                };
              
                await _context.AjoMembers.AddAsync(userAjoMapping);
                await _context.SaveChangesAsync(cancellationToken);


                await _context.CommitTransactionAsync();

                var sendOtpRequest = new CreateOTPCommand
                {
                    Email = request.Email,
                    Reason = "Email Verification"
                };
                var handler = await new CreateOTPCommandHandler(_context, _identityService, _emailService,_configuration).Handle(sendOtpRequest, cancellationToken);
                if (!handler.Succeeded)
                {
                    return Result.Failure("Reset Password Initiation failed:" + handler.Message);
                }
                string webDomain = _configuration["WebDomain"];
                //var link = $"<a href='{webDomain}login?token={newUser.Token}&AjoId={ajo.Id}'>Verify</a>";
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
                //    Otp=handler.Entity.ToString()
                //};
                //await _emailService.SendOtp(email2);
                return Result.Success(userAjoMapping);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "SignUp was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
