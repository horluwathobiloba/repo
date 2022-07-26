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
using System.Linq;

namespace OnyxDoc.AuthService.Application.GenerateUserInviteLink.Commands
{
    public class SendInviteToMultipleUsersCommand : IRequest<Result>
    {
        public string UserId { get; set; }
        public Dictionary<string,int> InvitedRecipients { get; set; }
        public int SubscriberId { get; set; }
    }
    public class SendInviteToMultipleUsersCommandHandler : IRequestHandler<SendInviteToMultipleUsersCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;
        private readonly IGenerateUserInviteLinkService _generateUserInviteLinkService;
        private readonly IConfiguration _configuration;
        public SendInviteToMultipleUsersCommandHandler(IApplicationDbContext context, IMapper mapper,  IConfiguration configuration, IGenerateUserInviteLinkService generateUserInviteLinkService, IEmailService emailService, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
            _mapper = mapper;
            _emailService = emailService;
            _configuration = configuration;
            _generateUserInviteLinkService = generateUserInviteLinkService;
        }
        public async Task<Result> Handle(SendInviteToMultipleUsersCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _context.BeginTransactionAsync();
               
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

                var entity = new List<UserInviteLink>();
                var usersList = new List<User>();
                var list = new List<EmailVm>();
                var subscriberRoles = await _context.Roles.Where(a=>a.SubscriberId == request.SubscriberId).ToListAsync();
                if (subscriberRoles == null)
                {
                    return Result.Failure("No valid roles exist in this organisation.");
                }

                foreach (var invite in request.InvitedRecipients)
                {
                    
                    var linkEntity = new UserInviteLink
                    {
                        RoleId = invite.Value,
                        SubscriberId = request.SubscriberId,
                        UserId = request.UserId,
                        UserEmail = user.user.Email,
                        RecipientEmail = invite.Key,
                        CreatedDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                    };
                    if (subscriberRoles.FirstOrDefault(a => a.Id == invite.Value) == null)
                    {
                        linkEntity.RoleId = subscriberRoles.FirstOrDefault(a=>(int)a.RoleAccessLevel == invite.Value).Id;
                    }
                    var generatedLink = await _generateUserInviteLinkService.GenerateUserInviteLink(linkEntity);
                    linkEntity.Link = generatedLink;
                    entity.Add(linkEntity);

                    var randomDigit = new Random();
                    //create user.
                    var userObject = new User
                    {
                        Email = linkEntity.RecipientEmail,
                        RoleId = linkEntity.RoleId,
                        SubscriberId = linkEntity.SubscriberId,
                        Password = "@OnyxDoc_"+ randomDigit.Next().ToString().Substring(0, 6),
                        FirstName = linkEntity.RecipientEmail,
                        LastName = linkEntity.RecipientEmail,
                        Name = linkEntity.RecipientEmail,
                        CreatedDate = DateTime.Now,
                        CreatedById = request.UserId,
                        CreatedByEmail = user.user.Email,
                        UserCreationStatus = Domain.Enums.UserCreationStatus.Invited,
                        UserCreationStatusDesc = Domain.Enums.UserCreationStatus.Invited.ToString()
                    };
                    usersList.Add(userObject);

                    //call email service
                    string webDomain = _configuration["WebDomain"];
                    string inviteLink = webDomain + $"invite?email={invite.Key}&token={generatedLink}'";
                    //send email to the recipient emails
                    list.Add(new EmailVm
                    {
                        Application = "OnyxDoc",
                        Subject = "Invite User",
                        BCC = "",
                        CC = "",
                        Text = "",
                        RecipientEmail = invite.Key,
                        FirstName = invite.Key,
                        Body = $"{subscriber.Name} would like you to join their team!",
                        Body1 = "Click the button to accept invite.",
                        ButtonText = "Accept Invite",
                        ButtonLink = inviteLink,
                        RecipientName = invite.Key
                    });
                }

                //create the user
                var result = await _identityService.CreateUsersAsync(usersList);
                if (!result.Succeeded)
                {
                    return Result.Failure(result.Message + " "+ result.Messages);
                }
                await _emailService.SendBulkInviteEmail(list);
                await _context.UserInviteLinks.AddRangeAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);

                await _context.CommitTransactionAsync();

                return Result.Success("Invite email sent successfully to the recipients.");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Sending Invite to multiple users was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
