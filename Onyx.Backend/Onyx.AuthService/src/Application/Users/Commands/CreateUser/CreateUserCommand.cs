using Onyx.AuthService.Application.Common.Interfaces;
using Onyx.AuthService.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Onyx.AuthService.Domain.Enums;
using System;
using Onyx.AuthService.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AutoMapper;
using Onyx.AuthService.Application.Users.Queries.GetUsers;
using Microsoft.Extensions.Configuration;

namespace Onyx.AuthService.Application.Users.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<Result>
    {
        public int OrganizationId { get; set; }
        public string FirstName { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public Gender Gender { get; set; }
        public int RoleId { get; set; }
        public int JobfunctionId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime EmploymentDate { get; set; }
        public string UserId { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string ProfilePicture { get; set; }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IBase64ToFileConverter _base64ToFileConverter;
        private readonly IStringHashingService _stringHashingService;

        public CreateUserCommandHandler(IApplicationDbContext context, IIdentityService identityService,
            IMapper mapper, IEmailService emailService, IConfiguration configuration,
            IBase64ToFileConverter base64ToFileConverter, IStringHashingService stringHashingService)
        {
            _context = context;
            _identityService = identityService;
            _mapper = mapper;
            _emailService = emailService;
            _configuration = configuration;
            _base64ToFileConverter = base64ToFileConverter;
            _stringHashingService = stringHashingService;
        }

        public async Task<Result> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {

                var organization = await _context.Organizations.Where(a => a.Id == request.OrganizationId).FirstOrDefaultAsync();
                if (organization == null)
                {
                    return Result.Failure("Invalid Organization Specified");
                }

                var userCheck = await _identityService.GetUserByIdAndOrganization(request.UserId, request.OrganizationId);
                if (userCheck.staff is null)
                {
                    return Result.Failure(new string[] { "Invalid User" });
                }

                var staffDetail = await _identityService.GetUserByUsername(request.Email);
                if (staffDetail.staff != null)
                {
                    //check if staff exist in this organisation
                    if (staffDetail.staff.OrganizationId == request.OrganizationId)
                    {
                        return Result.Failure(new string[] { "User already exists with this email" });
                    }
                }
               
               
                var lastCount = await _context.UserCount.Where(a => a.OrganizationId == request.OrganizationId).FirstOrDefaultAsync();
                if (lastCount == null)
                {
                    lastCount = new UserCount();
                    lastCount.Count = 0;
                    lastCount.OrganizationId = request.OrganizationId;
                }
                var staff = new User
                {
                    Address = request.Address.Trim(),
                    DateOfBirth = request.DateOfBirth,
                    Email = request.Email.Trim(),
                    FirstName = request.FirstName.Trim(),
                    Gender = request.Gender,
                    LastName = request.LastName.Trim(),
                    OrganizationId = request.OrganizationId,
                    Password = request.Password.Trim(),
                    UserName = request.Email.Trim(),
                    RoleId = request.RoleId,
                    JobFunctionId=request.JobfunctionId,
                    Status = Status.Inactive,
                    CreatedDate = DateTime.Now,
                    CreatedBy = "",
                    Country = request.Country,
                    State = request.State,
                    EmploymentDate = request.EmploymentDate,
                    ProfilePicture = await _base64ToFileConverter.ConvertBase64StringToFile(request.ProfilePicture, request.FirstName + "_" + request.LastName + ".png"),
                    StatusDesc = Status.Inactive.ToString(),
                    PhoneNumber = request.PhoneNumber,
                    
                };
                string webDomain = _configuration["WebDomain"];
                var hashValue = (staff.Email + DateTime.Now).ToString();
                staff.Token = _stringHashingService.CreateMD5StringHash(hashValue);
                var email = new EmailVm
                {
                    Application = "Onyx",
                    Subject = "Create User",
                    BCC = "",
                    CC = "",
                    Text = "",
                    RecipientEmail = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Password = request.Password,
                    OrganizationCode = organization.Code,
                    Body = "Your account has been created successfully!",
                    Body1 = "Click the button below to verify your account.",
                    ButtonText = "Verify Your Account",
                    ButtonLink = webDomain + $"login?email={request.Email}&token={staff.Token}'"

                };

                staff.Name = string.Concat(staff.FirstName, " ", staff.LastName);
                lastCount.Count += 1;
                staff.UserCode = "00" + lastCount.Count;
                var result = await _identityService.CreateUserAsync(staff);
                if (!result.Result.Succeeded)
                {
                    return Result.Failure(result.Result.Messages);
                }
                _context.UserCount.Update(lastCount);
                //_context..Add(staff);
                await _emailService.EmailVerification(email);
                await _context.SaveChangesAsync(cancellationToken);
                staff.UserId = result.UserId;
                var staffResult = _mapper.Map<UserDto>(staff);
                return Result.Success("User creation was successful", staffResult);
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "User creation was not successful", ex?.Message ?? ex?.InnerException.Message });
            }

        }
    }
}
