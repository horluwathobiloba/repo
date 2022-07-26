using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.AuthService.Application.GenerateInviteLink.Commands
{
    public class ConfirmInviteLinkCommand:IRequest<Result>
    {
        public string Link { get; set; }
    }
    public class ConfirmInviteLinkCommandHandler : IRequestHandler<ConfirmInviteLinkCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IGenerateUserInviteLinkService _generateUserInviteLinkService;
        public ConfirmInviteLinkCommandHandler(IApplicationDbContext context, IGenerateUserInviteLinkService generateUserInviteLinkService)
        {
            _context = context;
            _generateUserInviteLinkService = generateUserInviteLinkService;
        }
        public async Task<Result> Handle(ConfirmInviteLinkCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //confirm user link
                var link = await _generateUserInviteLinkService.ConfirmUserLink(request.Link);
                var entity = await _context.UserInviteLinks.FirstOrDefaultAsync(x => x.Link == request.Link && x.RecipientEmail==link.RecipientEmail);
                if (entity==null)
                {
                    return Result.Failure("This link does not exist");
                }
                var userDetails = new
                {
                    RoleId = link.RoleId,
                    SubscriberId = link.SubscriberId,
                    RecipientEmail = link.RecipientEmail
                };
                return Result.Success("link confirmation successful", userDetails);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Invite link confirmation was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
