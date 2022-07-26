using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Application.Common.Models;
using OnyxDoc.AuthService.Application.Users.Commands.CreateUser;
using OnyxDoc.AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.AuthService.Application.SignUp
{
    public class JoinExistingOrganizationCommand:IRequest<Result>
    {
        public string Email { get; set; }
        public int SubscriberId { get; set; }
    }
    public class JoinExistingOrganizationCommandHandler : IRequestHandler<JoinExistingOrganizationCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IIdentityService _identityService;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly IBase64ToFileConverter _base64ToFileConverter;
        private readonly IGenerateUserInviteLinkService _generateUserInviteLinkService;
        private readonly IStringHashingService _stringHashingService;
        private readonly IDomainCheckService _domainCheckService;
        public JoinExistingOrganizationCommandHandler(IApplicationDbContext context, IMapper mapper, IEmailService emailService, 
                                   IBase64ToFileConverter base64ToFileConverter,IIdentityService identityService, IGenerateUserInviteLinkService generateUserInviteLinkService,
                                   IConfiguration configuration, IStringHashingService stringHashingService, IDomainCheckService domainCheckService)
        {
            _context = context;
            _identityService = identityService;
            _emailService = emailService;
            _configuration = configuration;
            _mapper = mapper;
            _base64ToFileConverter = base64ToFileConverter;
            _generateUserInviteLinkService = generateUserInviteLinkService;
            _stringHashingService = stringHashingService;
            _domainCheckService = domainCheckService;
    }
        public async Task<Result> Handle(JoinExistingOrganizationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var domain = request.Email.Split("@", StringSplitOptions.None)[1];
                var existingDomain = await _domainCheckService.DomainExists(_context, Domain.Enums.SubscriberType.Corporate, domain);
                if (!existingDomain.Any())
                {
                    return Result.Failure("Domain name does not exist!!");
                }
                //that means it is true.
                var subscriber = await _context.Subscribers.FirstOrDefaultAsync(a => a.ContactEmail.Contains(domain));
                if (subscriber==null)
                {
                    return Result.Failure("This subscriber does not exist");
                }

                //check roles.
                var role = await _context.Roles.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId && x.RoleAccessLevel == Domain.Enums.RoleAccessLevel.Admin);
                if (role == null)
                {
                    return Result.Failure("The admin role does not exist");
                }
                var user = await _identityService.GetUserById(role.CreatedById);
                if (user.user==null)
                {
                    user = await _identityService.GetUserByEmail(role.CreatedById);
                    if (user.user == null)
                    {
                        return Result.Failure("The admin user does not exist");
                    }
                    
                }
                //create user.
                var normalUserRole = await _context.Roles.FirstOrDefaultAsync(a=>a.SubscriberId == request.SubscriberId && a.RoleAccessLevel == Domain.Enums.RoleAccessLevel.User);
                var randomDigit = new Random();
                var handler = new CreateUserCommandHandler(_context, _identityService, _mapper, _emailService, _configuration, _base64ToFileConverter, _stringHashingService);
                var command = new CreateUserCommand
                {
                    Email = request.Email,
                    RoleId = normalUserRole.Id,
                    SubscriberId = request.SubscriberId,
                    Password = "@OnyxDoc_" + randomDigit.Next().ToString().Substring(0, 6),
                    FirstName = request.Email,
                    LastName = request.Email,
                    UserCreationStatus = Domain.Enums.UserCreationStatus.AccessRequest
                };

                var userResponse = await handler.Handle(command, cancellationToken);
                if (userResponse.Entity == null)
                {
                    return Result.Failure(userResponse.Message + userResponse.Messages);
                }

                var userEntity = userResponse.Entity;
                //send email to the admin
                string webDomain = _configuration["WebDomain"];
                var email = new EmailVm
                {
                    Application = "OnyxDoc",
                    Subject = "Request to Join your Organization",
                    BCC = "",
                    CC = "",
                    Text = "",
                    RecipientEmail = user.user.Email,
                    SubscriberName = subscriber.Name,
                    //Password = request.Password,
                    Body = "A user wants to join your team!",
                    Body1 = "Click the button to accept or reject user.",
                    ButtonText = "Login To Process Request",
                    ButtonLink = webDomain
                };
                await _emailService.AdminEmailVerification(email);

                //add details to the db.
                var entity = new UserApproverRequest
                {
                    UserEmail=request.Email,
                    SubscriberId=user.user.SubscriberId,
                    ApproverEmail=user.user.Email,
                    CreatedByEmail=user.user.Email,
                    UserCreationStatus=Domain.Enums.UserCreationStatus.AccessRequest,
                    UserCreationStatusDesc= Domain.Enums.UserCreationStatus.AccessRequest.ToString(),
                    LastModifiedDate=DateTime.Now,
                    CreatedDate=DateTime.Now
                };
                await _context.UserApproverRequests.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Email has been sent to the admin of the organization");

            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Joining an existing organisation was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
