using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.Entities;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.Users.Command.SendInvitationCode
{
    public class SendInvitationCodeCommand:IRequest<Result>
    {
        public int CooperativeId { get; set; }
        public string RecipientEmail { get; set; }
    }

    public class SendInvitationCodeCommandHandler : IRequestHandler<SendInvitationCodeCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IEmailService _emailService;
        private readonly IIdentityService _identityService;

        public SendInvitationCodeCommandHandler(IApplicationDbContext context,IEmailService emailService, IIdentityService identityService)
        {
            _context = context;
            _emailService = emailService;
            _identityService = identityService;
        }
        public async Task<Result> Handle(SendInvitationCodeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var cooperative = await _context.Cooperatives.FirstOrDefaultAsync(a => a.Id == request.CooperativeId);
                if (cooperative is null)
                {
                    return Result.Failure("Cooperative cannot be found");
                }
                var code = await _identityService.GenerateCooperativeCode(cooperative.Id, request.RecipientEmail);
                await _context.CooperativeUserCodes.AddAsync(code.coopCode);
                await _context.SaveChangesAsync(cancellationToken);
                if (code.success)
                {
                    var invite = new CooperativeInvitationTracker
                    {
                        CooperativeId = code.coopCode.CooperativeId,
                        CreatedDate = DateTime.Now,
                        StatusDesc = Status.Inactive.ToString(),
                        Status = Status.Inactive,
                        UserEmail = code.coopCode.Email,
                        RequestType = RequestType.Link,
                    };
                    var email = new EmailVm
                    {
                        Application = "Ruby",
                        Subject = "Create User With Link",
                        Text = "Create With Link",
                        RecipientEmail = request.RecipientEmail,
                        Body1 = $"Use This Code To Sign Up to {cooperative.Name} cooperative",
                        Otp = code.coopCode.Code
                    };
                    await _emailService.SendOtp(email);
                    return Result.Success(email.Otp);
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
