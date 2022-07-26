using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Application.Common.Models;
using OnyxDoc.AuthService.Domain.Entities;
using System;
using ReventInject;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OnyxDoc.AuthService.Application.Users.Commands.CreateUser;
using AutoMapper;
using OnyxDoc.AuthService.Domain.Enums;

namespace OnyxDoc.AuthService.Application.GenerateUserInviteLink.Commands
{
    public class GenerateUserInviteLinkCommand : IRequest<Result>
    {
        public int RoleId { get; set; }
        public string UserId { get; set; }
        public string RecipientEmail { get; set; }
        public int SubscriberId { get; set; }
        public bool IsLinkCopied { get; set; }
    }
    public class GenerateUserInviteLinkCommandHandler : IRequestHandler<GenerateUserInviteLinkCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;
        private readonly IGenerateUserInviteLinkService _generateUserInviteLinkService;
        private readonly IBase64ToFileConverter _base64ToFileConverter;
        private readonly IStringHashingService _stringHashingService;
        private readonly IConfiguration _configuration;
        public GenerateUserInviteLinkCommandHandler(IApplicationDbContext context, IMapper mapper,IStringHashingService stringHashingService, IBase64ToFileConverter base64ToFileConverter, IConfiguration configuration, IGenerateUserInviteLinkService generateUserInviteLinkService, IEmailService emailService, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
            _mapper = mapper;
            _stringHashingService = stringHashingService;
            _base64ToFileConverter = base64ToFileConverter;
            _emailService = emailService;
            _configuration = configuration;
            _generateUserInviteLinkService = generateUserInviteLinkService;
        }
        public async Task<Result> Handle(GenerateUserInviteLinkCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _context.BeginTransactionAsync();

                var role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == request.RoleId);
                if (role == null)
                {
                    return Result.Failure("This role does not exist.");
                }
                var user = await _identityService.GetUserById(request.UserId);
                if (user.user == null)
                {
                    return Result.Failure("User does not exist");
                }
                var subscriber = await _context.Subscribers.FirstOrDefaultAsync(x => x.Id == request.SubscriberId);
                if (subscriber == null)
                {
                    return Result.Failure("This subscriber does not exist");
                }
                string webDomain = _configuration["WebDomain"];
                var linkEntity = new UserInviteLink
                {
                    RoleId = request.RoleId,
                    SubscriberId = request.SubscriberId,
                    UserId = request.UserId,
                    UserEmail = user.user.Email,
                    RecipientEmail = request.RecipientEmail,
                    CreatedDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                };

                //check if the recipient email is null, if null return link
                if (string.IsNullOrEmpty(request.RecipientEmail))
                {
                    var userLink = await _generateUserInviteLinkService.GenerateUserLink(linkEntity);
                    linkEntity.Link = userLink;
                    await _context.UserInviteLinks.AddAsync(linkEntity);
                    await _context.SaveChangesAsync(cancellationToken);
                    return Result.Success("Link generated successfully!!", webDomain + $"invite?email={request.RecipientEmail}&token={userLink}'");
                }

                //call the generateLink service
                var generatedLink = await _generateUserInviteLinkService.GenerateUserInviteLink(linkEntity);
                linkEntity.Link = generatedLink;
                await _context.UserInviteLinks.AddAsync(linkEntity);
                await _context.SaveChangesAsync(cancellationToken);

                //create user.
                var randomDigit = new Random();
                var handler = new CreateUserCommandHandler(_context, _identityService, _mapper, _emailService, _configuration, _base64ToFileConverter, _stringHashingService);
                var command = new CreateUserCommand
                {
                    Email = linkEntity.RecipientEmail,
                    RoleId = linkEntity.RoleId,
                    SubscriberId = linkEntity.SubscriberId,
                    Password = "@OnyxDoc_" + randomDigit.Next().ToString().Substring(0, 6),
                    FirstName = linkEntity.RecipientEmail,
                    LastName = linkEntity.RecipientEmail,
                    UserCreationStatus = UserCreationStatus.Invited,
                    
            };

                var userResponse = await handler.Handle(command, cancellationToken);
                if (userResponse.Entity == null)
                {
                    return Result.Failure(userResponse.Message);
                }

                //var userEntity = (User)userResponse.Entity;


                string inviteLink = webDomain + $"invite?email={request.RecipientEmail}&token={generatedLink}'";

                //call email service
                if (!request.IsLinkCopied)
                {
                    var email = new EmailVm
                    {
                        Application = "OnyxDoc",
                        Subject = "Invite User",
                        BCC = "",
                        CC = "",
                        Text = "",
                        RecipientEmail = request.RecipientEmail,
                        FirstName = request.RecipientEmail,
                        Body = $"{subscriber.Name} would like you to join their team!",
                        Body1 = "Click the button to accept invite.",
                        ButtonText = "Accept Invite",
                        ButtonLink = inviteLink,
                        RecipientName = request.RecipientEmail
                    };
                    await _emailService.AdminEmailVerification(email); 
                }
                await _context.CommitTransactionAsync();
                return Result.Success("Link generated successfully",inviteLink);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "User generation link was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
