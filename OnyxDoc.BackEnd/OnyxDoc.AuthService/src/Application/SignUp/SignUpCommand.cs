using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using OnyxDoc.AuthService.Application.Common.Models;
using OnyxDoc.AuthService.Domain.Enums;
using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Domain.Entities;
using OnyxDoc.AuthService.Application.Subscribers.Commands.CreateSubscriber;
using AutoMapper;
using OnyxDoc.AuthService.Application.Users.Commands.CreateUser;
using OnyxDoc.AuthService.Application.Roles.Commands.CreateRole;
using OnyxDoc.AuthService.Application.GenerateUserInviteLink.Commands;
using OnyxDoc.AuthService.Application.Users.Queries.GetUsers;
using OnyxDoc.AuthService.Application.Roless.Commands.CreateRoles;
using OnyxDoc.AuthService.Application.Roles.Queries.GetRoles;
using OnyxDoc.AuthService.Application.Roles.Commands.CreateDefaultRoleAndPermissions;
using OnyxDoc.AuthService.Application.Subscribers.Queries.GetSubscribers;
using OnyxDoc.AuthService.Application.RolePermissions.Queries.GetRolePermissions;
using OnyxDoc.AuthService.Application.Roles.Commands.CreateRoleAndPermissions;
using OnyxDoc.AuthService.Application.RolePermissions.Commands.CreateRolePermissions;

namespace OnyxDoc.AuthService.Application.Commands.SignUp
{
    public class SignUpCommand :  IRequest<Result>
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string ContactEmail { get; set; }
        public string StaffSize { get; set; }
        public string Industry { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Referrer { get; set; }
        public string FirstName { get; set; }
        public string JobTitle { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public Dictionary<string,int> InvitedRecipients { get; set; }
        public string ThemeColor { get; set; }
        public string ThemeColorCode { get; set; }
        public string ProfilePicture { get; set; }
        public string SubscriptionPurpose { get; set; }
        public SubscriberType SubscriberType { get; set; }
        public SubscriberAccessLevel SubscriberAccessLevel { get; set; } 
    }

    public class SignUpCommandHandler : IRequestHandler<SignUpCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly ISqlService _sqlService;
        private readonly IStringHashingService _stringHashingService;
        private readonly IMapper _mapper;
        private readonly IBase64ToFileConverter _base64ToFileConverter;
        private readonly IGenerateUserInviteLinkService _generateUserInviteLinkService;

        public SignUpCommandHandler(IApplicationDbContext context, IIdentityService identityService, 
            IBase64ToFileConverter base64ToFileConverter, IEmailService emailService, IConfiguration  configuration,
            ISqlService sqlService, IStringHashingService stringHashingService,
            IGenerateUserInviteLinkService generateUserInviteLinkService, IMapper mapper, IAuthenticateService authenticateService)
        {
            _context = context;
            _generateUserInviteLinkService = generateUserInviteLinkService;
            _identityService = identityService;
            _base64ToFileConverter = base64ToFileConverter;
            _mapper = mapper;
            _emailService = emailService;
            _configuration = configuration;
            _sqlService = sqlService;
            _stringHashingService = stringHashingService;
        }

        public async Task<Result> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _context.BeginTransactionAsync();
                var handler = new CreateSubscriberCommandHandler(_context,_identityService, _mapper, _emailService, _configuration, _base64ToFileConverter, _stringHashingService);

                var command = new CreateSubscriberCommand
                {
                    Name = request.Name,
                    PhoneNumber = request.PhoneNumber,
                    ContactEmail = request.ContactEmail,
                    StaffSize = request.StaffSize,
                    Industry = request.Industry,
                    Address = request.Address,
                    City = request.City,
                    Country = request.Country,
                    Latitude = request.Latitude,
                    Longitude = request.Longitude,
                    Referrer = request.Referrer,
                    State = request.State,
                    SubscriptionPurpose = request.SubscriptionPurpose,
                    SubscriberAccessLevel = request.SubscriberAccessLevel,
                    SubscriberType = request.SubscriberType,
                    ThemeColor = request.ThemeColor
                   
                };

                var subscriberResponse = await handler.Handle(command, cancellationToken);
                if (subscriberResponse.Entity==null)
                {
                   
                    return Result.Failure( string.IsNullOrEmpty(subscriberResponse.Message)? subscriberResponse.Messages: new string[] { subscriberResponse.Message });
                }

               

                //Get system details
                var getSystemSubscriber = await _context.Subscribers.Where(x => x.SubscriberAccessLevel == Domain.Enums.SubscriberAccessLevel.System).FirstOrDefaultAsync();
                

                var roles = new List<RolesVm>();
                var defaultRoleEntity = await _context.DefaultRolesConfigurations.Where(x => x.SubscriberType == request.SubscriberType).ToListAsync();
                if(defaultRoleEntity == null || defaultRoleEntity.Count() == 0)
                {
                    return Result.Failure("Default role configuration for required subscriber type does not exist");
                }
               
                foreach (var defaultRole in defaultRoleEntity)
                {
                    
                    if (defaultRole.RoleName == Domain.Enums.DefaultRoles.User)
                    {
                        var userRole = new RolesVm
                        {
                            Name = defaultRole.RoleName.ToString(),
                            RoleAccessLevel = defaultRole.RoleAccessLevel,
                        };
                        roles.Add(userRole);
                    }

                    if (defaultRole.RoleName == Domain.Enums.DefaultRoles.Manager)
                    {
                        var managerRole = new RolesVm
                        {
                            Name = defaultRole.RoleName.ToString(),
                            RoleAccessLevel = defaultRole.RoleAccessLevel,
                        };
                        roles.Add(managerRole);

                    }

                    if (defaultRole.RoleName == Domain.Enums.DefaultRoles.Admin)
                    {
                        var adminRole = new RolesVm
                        {
                            Name = defaultRole.RoleName.ToString(),
                            RoleAccessLevel = defaultRole.RoleAccessLevel,
                        };
                        roles.Add(adminRole);
                    }
                    
                };
   

                

                //TODO: FETCH FROM DEAFUTLT ROLES
               
                var subscriber = (Subscriber)subscriberResponse.Entity;
                
                //TODO: USE ROLE AND PERMISSIONS COMMAND

                var rolesHandler = new CreateRolesCommandHandler(_context, _identityService);
                var rolesCommand = new CreateRolesCommand
                {
                    RolesVm = roles,
                    SubscriberId = subscriber.Id,
                    UserId = request.Email

                };


                var roleResponse = await rolesHandler.Handle(rolesCommand, cancellationToken);
                if (roleResponse.Entity == null)
                {
                    return Result.Failure(subscriberResponse.Message);
                }
                var role = (List<Role>)roleResponse.Entity;
                //Add user object
                var userHandler = new CreateUserCommandHandler(_context, _identityService, _mapper, _emailService, _configuration, _base64ToFileConverter, _stringHashingService);
                var randomDigit = new Random();
                var user = new CreateUserCommand
                {
                    Email = request.Email.Trim(),
                    FirstName = request.FirstName.Trim(),
                    LastName = request.LastName.Trim(),
                    SubscriberId = subscriber.Id,
                    Password = string.IsNullOrEmpty(request.Password) ? "@OnyxDoc_" + randomDigit.Next().ToString().Substring(0, 6) :request.Password.Trim(),
                    RoleId = role.FirstOrDefault().Id,
                    Country = request.Country,
                    City = request.City,
                    JobTitle = request.JobTitle,
                    ProfilePicture = string.IsNullOrWhiteSpace(request.ProfilePicture) ? null: await _base64ToFileConverter.ConvertBase64StringToFile(request.ProfilePicture, request.FirstName + "_" + request.LastName + ".png"),
                    PhoneNumber = request.PhoneNumber
                };
                var userResponse = await userHandler.Handle(user, cancellationToken);
                if (userResponse.Entity==null)
                {
                    string messages = "";
                    foreach (var message in userResponse.Messages)
                    {
                        messages += " "+ message;
                    }
                    return Result.Failure(userResponse.Message + messages);
                }

                //get user details
                var userDetails = (UserDto)userResponse.Entity;

                //subscriber update
                var subscriberDetails = (Subscriber)subscriberResponse.Entity;
                subscriberDetails.CreatedById = userDetails.UserId;
                subscriberDetails.CreatedByEmail = userDetails.Email;
                _context.Subscribers.Update(subscriberDetails);

                //New
                if (request.ThemeColor != null)
                {
                    var branding = new Domain.Entities.Branding();
                    branding.ThemeColor = request.ThemeColor;
                    branding.ThemeColorCode = request.ThemeColorCode;
                    branding.SubscriberId = subscriberDetails.Id;
                    branding.Status = Status.Active;
                    branding.StatusDesc = Status.Active.ToString();
                    branding.CreatedById = userDetails.UserId;
                    branding.CreatedByEmail = userDetails.Email;
                    await _context.Brandings.AddAsync(branding);
                    await _context.SaveChangesAsync(cancellationToken);

                }
                //Call send invite to multiple user
                if (request.InvitedRecipients != null && request.InvitedRecipients.Count > 0)
                {
                    var inviteeHandler = new SendInviteToMultipleUsersCommandHandler(_context, _mapper, _configuration, _generateUserInviteLinkService, _emailService, _identityService);
                    var inviteUsers = new SendInviteToMultipleUsersCommand
                    {
                        InvitedRecipients = request.InvitedRecipients,
                        UserId = userDetails.UserId,
                        SubscriberId = subscriber.Id
                    };
                    var inviteeResponse = await inviteeHandler.Handle(inviteUsers, cancellationToken);
                    if (!inviteeResponse.Succeeded)
                    {
                        return Result.Failure(inviteeResponse.Message +"Error creating recipient users");
                    } 
                }
                var entity = new
                {
                    User = userResponse.Entity,
                    Role = roleResponse.Entity,
                    SubscriberResponse = subscriberResponse.Entity
                };
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                 return Result.Success("Signup was successful", entity);
            }
            catch (Exception ex)
            {
                 _context.RollbackTransaction();
                return Result.Failure(new string[] { "Sign up was not successful", ex?.Message??ex?.InnerException.Message });
            }
        }
    }
}


