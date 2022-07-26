
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.Entities;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;

using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.User.Command.GetInvitationLink
{
    public class CreateInvitationLinkCommand : IRequest<Result>
    {
        public List<string> RecipientEmail { get; set; }
        public string LoggedInUserId { get; set; }
        public int CooperativeId { get; set; }
  
    }

    public class CreateInvitationLinkCommandHandler : IRequestHandler<CreateInvitationLinkCommand, Result>
    {
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        private readonly IApplicationDbContext _context;
        private readonly IEmailService _emailService;
        private readonly IIdentityService _identityService;
        public CreateInvitationLinkCommandHandler(IConfiguration configuration,
            ITokenService tokenService,IApplicationDbContext context,IEmailService emailService, 
            IIdentityService identityService)
        {
            _configuration = configuration;
            _context = context;
            _tokenService = tokenService;
            _emailService = emailService;
            _identityService = identityService;
        }
        public async Task<Result> Handle(CreateInvitationLinkCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var cooperative = await _context.Cooperatives.FirstOrDefaultAsync(x => x.Id == request.CooperativeId);
                string webDomain = _configuration["WebDomain"];
                var code = await _identityService.GenerateCooperativeCodes(cooperative.Id, request.RecipientEmail);
                await _context.CooperativeUserCodes.AddRangeAsync(code.coopCodes);
                await _context.SaveChangesAsync(cancellationToken);
                var links = new List<string>();
                var invitations = new List<CooperativeInvitationTracker>();
                foreach (var coopCode in code.coopCodes)
                {
                    var token = await _tokenService.GenerateInviteToken(coopCode.Email, coopCode.Code, request.CooperativeId.ToString(),Domain.Enums.LinkType.Cooperative);
                    var link = $"{webDomain}login?email={coopCode.Email}&token={token.AccessToken}'";
                    var invite = new CooperativeInvitationTracker
                    {
                        CooperativeId = coopCode.CooperativeId,
                        CreatedDate = DateTime.Now,
                        StatusDesc = Status.Inactive.ToString(),
                        Status = Status.Inactive,
                        UserEmail = coopCode.Email,
                        RequestType = RequestType.Link,
                    };
                    invitations.Add(invite);
                    var email = new EmailVm
                    {
                        Application = "Ruby",
                        Subject = "Create User With Link",
                        Text = "Create With Link",
                        RecipientEmail = coopCode.Email,
                        Body1 = "Use This Link To Sign Up",
                        ButtonLink = link,
                        ButtonText = "Click the Link"
                    };
                    await _emailService.InvitationLink(email);
                    links.Add(link);

                }
                await _context.CooperativeInvitationTrackers.AddRangeAsync(invitations);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success(links);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Invite link creation was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
           
        }
    }
}
