using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.Entities;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.Ajos.Command.CreateAjoInviteLink
{
    public class CreateAjoInviteLinkCommand:IRequest<Result>
    {
       
        public List<string> RecipientEmails { get; set; }
        //public string Code { get; set; }
        public int AjoId { get; set; }
        //public string UserId { get; set; }
        
    }

    public class CreateAjoInviteLinkCommandHandler : IRequestHandler<CreateAjoInviteLinkCommand, Result>
    {
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IEmailService _emailService;
        public CreateAjoInviteLinkCommandHandler(IConfiguration configuration, ITokenService tokenService, 
            IApplicationDbContext context, IIdentityService identityService, IEmailService emailService)
        {
            _configuration = configuration;
            _context = context;
            _tokenService = tokenService;
            _identityService = identityService;
            _emailService = emailService;
        }
        public async Task<Result> Handle(CreateAjoInviteLinkCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var ajo = await _context.Ajos.FirstOrDefaultAsync(x => x.Id == request.AjoId);
                string webDomain = _configuration["WebDomain"];
                var code = await _identityService.GenerateAjoCodes(ajo.Id, request.RecipientEmails);
                await _context.AjoCodes.AddRangeAsync(code.ajoCodes);
                await _context.SaveChangesAsync(cancellationToken);

                var links = new List<string>();
                var invitees = new List<UserLinkInvite>();
                var invitations = new List<AjoInvitationTracker>();
                foreach (var ajocode in code.ajoCodes)
                {
                    var token = await _tokenService.GenerateInviteToken(ajocode.Email,ajocode.Code, request.AjoId.ToString(),Domain.Enums.LinkType.Ajo);
                    var link = $"{webDomain}login?email={ajocode.Email}&token={token.AccessToken}";
                    var invite = new AjoInvitationTracker
                    {
                        AjoId=ajocode.AjoId,
                        CreatedDate = DateTime.Now,
                        StatusDesc = Status.Inactive.ToString(),
                        Status = Status.Inactive,
                        UserEmail = ajocode.Email,
                        RequestType = RequestType.Link,
                    };
                    var email = new EmailVm
                    {
                        Application = "Ruby",
                        Subject = "Create User With Link",
                        Text = "Create With Link",
                        RecipientEmail = ajocode.Email,
                        FirstName=ajocode.Email,
                        Body1 = "Use This Link To Sign Up",
                        Body2="",
                        Body="",
                        Body3="",
                        ButtonLink = link,
                        ButtonText = "Click the Link"
                    };

                    await _emailService.InvitationLink(email);
                    links.Add(link);
                    invitations.Add(invite);
                }
                await _context.AjoInvitationTrackers.AddRangeAsync(invitations);
                await _context.SaveChangesAsync(cancellationToken);
               
                //var entity = new UserLinkInvite
                //{
                //    AjoId = request.AjoId.ToString(),
                //    Code =ajo.Code,
                //    CreatedDate = DateTime.Now,
                //    IsUsed = false,
                //    CreatedById = request.UserId,
                //    RecipientEmail = request.RecipientEmail,
                //    Token = token.AccessToken,
                //    UserId = request.UserId
                //};
            
                //var email = new EmailVm
                //{
                //    Application = "Ruby",
                //    Subject = "Create User With Link",
                //    Text = "Create With Link",
                //    RecipientEmail = request.RecipientEmail,
                //    Body1 = "Use This Link To Sign Up",
                //    ButtonLink = link,
                //    ButtonText = "Click the Link"
                ////};
             
                return Result.Success(links);

            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Invite link creation was not successful", ex?.Message ?? ex?.InnerException.Message });
            }

        }
    }
}
