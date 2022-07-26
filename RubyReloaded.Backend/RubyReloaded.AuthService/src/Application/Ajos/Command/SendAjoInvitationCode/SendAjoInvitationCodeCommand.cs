using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.Entities;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.Ajos.Command.SendAjoInvitationCode
{
    public class SendAjoInvitationCodeCommand : IRequest<Result>
    {
        public int AjoId { get; set; }
        public List<string> RecipientEmails { get; set; }
 
    }


    public class SendAjoInvitationCodeCommandHandler : IRequestHandler<SendAjoInvitationCodeCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IEmailService _emailService;
        private readonly IIdentityService _identityService;
        public SendAjoInvitationCodeCommandHandler(IApplicationDbContext context, IEmailService emailService, IIdentityService identityService)
        {
            _context = context;
            _emailService = emailService;
            _identityService = identityService;
        }
        public async Task<Result> Handle(SendAjoInvitationCodeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var ajo = await _context.Ajos.FirstOrDefaultAsync(a => a.Id == request.AjoId);
                if (ajo is null)
                {
                    return Result.Failure("ajo cannot be found");
                }
                var code = await _identityService.GenerateAjoCodes(ajo.Id, request.RecipientEmails);
                await _context.AjoCodes.AddRangeAsync(code.ajoCodes);
                await _context.SaveChangesAsync(cancellationToken);
                var invitations = new List<AjoInvitationTracker>();
                if (code.success)
                {
                    var recipientEmails = code.ajoCodes.Select(x => x.Email);
                    foreach (var ajocode in code.ajoCodes)
                    {
                        var email = new EmailVm
                        {
                            Application = "Ruby",
                            Subject = "Create User With Code",
                            Text = "Create With Link",
                            RecipientEmail = ajocode.Email,
                            Body1 = $"Use This Code To Join {ajo.Name} ajo",
                            Otp = ajocode.Code
                        };
                        var invite = new AjoInvitationTracker
                        {
                            AjoId = ajocode.AjoId,
                            CreatedDate = DateTime.Now,
                            StatusDesc = Status.Inactive.ToString(),
                            Status = Status.Inactive,
                            UserEmail = ajocode.Email,
                            RequestType = RequestType.Code,
                        };
                        invitations.Add(invite);
                        await _emailService.SendOtp(email);
                    }
                    await _context.AjoInvitationTrackers.AddRangeAsync(invitations);
                    await _context.SaveChangesAsync(cancellationToken);

                    return Result.Success("Invites sent successfully");
                }
                return Result.Failure("Failed to generate code");

            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Error sending link: ", ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }

}
