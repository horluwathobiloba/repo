using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Application.Common.Models;
using OnyxDoc.AuthService.Domain.Entities;
using OnyxDoc.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.AuthService.Application.Commands.SignUp
{
    public class ApproveOrRejectRequestCommand: IRequest<Result>
    {
        public string Email { get; set; }
        public int RoleId { get; set; }

        public int SubscriberId { get; set; }
        public string UserId { get; set; }
        public bool Response { get; set; }
    }
    public class ApproveOrRejectRequestCommandHandler : IRequestHandler<ApproveOrRejectRequestCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IIdentityService _identityService;
        private readonly IEmailService _emailService;
        private readonly IGenerateUserInviteLinkService _generateUserInviteLinkService;
        public ApproveOrRejectRequestCommandHandler(IApplicationDbContext context, IGenerateUserInviteLinkService generateUserInviteLinkService, IEmailService emailService, IIdentityService identityService,IConfiguration configuration)
        {
            _context = context;
            _generateUserInviteLinkService = generateUserInviteLinkService;
            _emailService = emailService;
            _configuration = configuration;
            _identityService = identityService;
        }
        public async Task<Result> Handle(ApproveOrRejectRequestCommand request, CancellationToken cancellationToken)
        {
            try
            {
              
                var user = await _identityService.GetUserById(request.UserId); 
                if (user.user == null)
                {
                    return Result.Failure("User does not exist");
                }
                var subscriber = await _context.Subscribers.FirstOrDefaultAsync(x => x.Id == user.user.SubscriberId);
                if (subscriber == null)
                {
                    return Result.Failure("Invalid Subscriber Details");
                }

                var entity = await _context.UserApproverRequests.FirstOrDefaultAsync(x => x.ApproverEmail == user.user.Email);
                if (entity == null)
                {
                    return Result.Failure("User is not an approver");
                }
                //update to add email
                var userToUpdate = await _identityService.GetUserByEmail(request.Email);
                if (userToUpdate.user == null)
                {
                    return Result.Failure("Invalid User Access Request !");
                }
                userToUpdate.user.RoleId = request.RoleId;
                await _identityService.UpdateUserAsync(userToUpdate.user);
                if (request.Response)
                {
                    var hashCode = new UserHashCode
                    {
                        UserEmail = entity.UserEmail,
                        Time = DateTime.Now.Ticks,
                        SubscriberId = entity.SubscriberId
                    };

                    //generate has token
                    var link = await _generateUserInviteLinkService.GenerateHashLink(hashCode);

                    //send email to the user that wants to join
                    string webDomain = _configuration["WebDomain"];
                    var email = new EmailVm
                    {
                        Application = "OnyxDoc",
                        Subject = "Request to Join your Organization",
                        BCC = "",
                        CC = "",
                        Text = "",
                        RecipientEmail = entity.UserEmail,
                        FirstName = entity.UserEmail,
                        Body = $"We saw that you requested to join {subscriber.Name}!",
                        Body1 = "Click the button to join the team.",
                        ButtonText = "Verify Your Account",
                        ButtonLink = webDomain + $"verify?email={entity.UserEmail}&token={link}",
                        RecipientName = user.user.FirstName
                    };
                    await _emailService.AdminEmailVerification(email);

                    entity.UserCreationStatus = Domain.Enums.UserCreationStatus.Approved;
                    _context.UserApproverRequests.Update(entity);
                    await _context.SaveChangesAsync(cancellationToken);

                    return Result.Success("Email sent to the user that request to join the team is successful");
                }

                entity.UserCreationStatus = Domain.Enums.UserCreationStatus.Rejected;

                await _identityService.DeleteUserAsync(userToUpdate.user.UserId);
                _context.UserApproverRequests.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success("Rejected sucessfully");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Approval or rejection was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
