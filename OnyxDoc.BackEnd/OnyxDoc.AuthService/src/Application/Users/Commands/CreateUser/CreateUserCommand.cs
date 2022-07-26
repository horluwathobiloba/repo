using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using OnyxDoc.AuthService.Domain.Enums;
using System;
using OnyxDoc.AuthService.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AutoMapper;
using OnyxDoc.AuthService.Application.Users.Queries.GetUsers;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;

namespace OnyxDoc.AuthService.Application.Users.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string FirstName { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string JobTitle { get; set; }
        public int RoleId { get; set; }
        public string UserId { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string ProfilePicture { get; set; }
        [JsonIgnore]
        public UserCreationStatus UserCreationStatus { get; set; }

        [JsonIgnore]
        public bool IsInvitedUser { get; set; }
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

                var subscriber = await _context.Subscribers.Where(a => a.Id == request.SubscriberId).FirstOrDefaultAsync();
                if (subscriber == null)
                {
                    return Result.Failure("Invalid Subscriber Specified");
                }

                if (!request.IsInvitedUser)
                {
                    var userCheck = await _identityService.GetUserByIdAndSubscriber(request.UserId, request.SubscriberId);
                    if (userCheck.user is null)
                    {
                        return Result.Failure("User does not exist in this organisation");
                    } 
                }

                var userDetail = await _identityService.GetUserByEmail(request.Email);
                if (userDetail.user != null)
                {
                    //check if user exist in this organisation
                    if (userDetail.user.SubscriberId == request.SubscriberId)
                    {
                        return Result.Failure("User already exists with this email" );
                    }
                }
               
               
                var lastCount = await _context.UserCount.Where(a => a.SubscriberId == request.SubscriberId).FirstOrDefaultAsync();
                if (lastCount == null)
                {
                    lastCount = new UserCount();
                    lastCount.Count = 0;
                    lastCount.SubscriberId = request.SubscriberId;
                }
                var user = new User
                {
                    Email = request.Email.Trim(),
                    FirstName = request.FirstName.Trim(),
                    LastName = request.LastName.Trim(),
                    SubscriberId = request.SubscriberId,
                    Password = request.Password.Trim(),
                    RoleId = request.RoleId,
                    CreatedDate = DateTime.Now,
                    CreatedByEmail = request.Email.Trim(),
                    Country = request.Country,
                    City = request.City,
                    JobTitle = request.JobTitle,
                    ProfilePicture = await _base64ToFileConverter.ConvertBase64StringToFile(request.ProfilePicture, request.FirstName + "_" + request.LastName + ".png"),
                    Status = Status.Inactive,
                    StatusDesc = Status.Inactive.ToString(),
                    PhoneNumber = request.PhoneNumber,
                    UserCreationStatus = UserCreationStatus.Approved,
                    UserCreationStatusDesc = UserCreationStatus.Approved.ToString()
                };
                if (request.IsInvitedUser)
                {
                    user.UserCreationStatus = UserCreationStatus.Invited;
                    user.UserCreationStatusDesc = UserCreationStatus.Invited.ToString();
                }
                if (request.UserCreationStatus > 0)
                {
                    user.UserCreationStatus = request.UserCreationStatus;
                    user.UserCreationStatusDesc = request.UserCreationStatus.ToString();
                }
                string webDomain = _configuration["WebDomain"];
                var hashValue = (user.Email + DateTime.Now).ToString();
                user.Token = _stringHashingService.CreateMD5StringHash(hashValue);
                var email = new EmailVm
                {
                    Application = "OnyxDoc",
                    Subject = "Create User",
                    BCC = "",
                    CC = "",
                    Text = "",
                    RecipientEmail = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Password = request.Password,
                    Body = "Your account has been created successfully!",
                    Body1 = "Click the button below to verify your account.",
                    ButtonText = "Verify Your Account",
                    ButtonLink = webDomain + $"verify?email={request.Email}&token={user.Token}'",
                    RecipientName=request.FirstName,
                    SubscriberId= request.SubscriberId,
                    SubscriberName = subscriber.Name
                };

                user.Name = string.Concat(user.FirstName, " ", user.LastName);
                lastCount.Count += 1;
                var result = await _identityService.CreateUserAsync(user);
                if (!result.Result.Succeeded)
                {
                    return Result.Failure(result.Result.Messages);
                }
                _context.UserCount.Update(lastCount);
                //_context..Add(user);
                await _emailService.EmailVerification(email);
                await _context.SaveChangesAsync(cancellationToken);
                user.UserId = result.UserId;
                var userResult = _mapper.Map<UserDto>(user);
                return Result.Success("User creation was successful", userResult);
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "User creation was not successful", ex?.Message ?? ex?.InnerException.Message });
            }

        }
    }
}
