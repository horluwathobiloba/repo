using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.Users.Command.VerifyInvitationCode
{
    public class VerifyInvitationCodeCommand:IRequest<Result>
    {
        public string Email { get; set; }
        public string Code { get; set; }
        public int CooperativeId { get; set; }
    }

    public class VerifyInvitationCodeCommandHandler : IRequestHandler<VerifyInvitationCodeCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        public VerifyInvitationCodeCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(VerifyInvitationCodeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var coopCode = await _context.CooperativeUserCodes.FirstOrDefaultAsync(x => x.Code == request.Code && x.Email == request.Email);

                if (coopCode.IsUsed)
                {
                    return Result.Failure("This code has already been used");
                }
                if (coopCode.CooperativeId != request.CooperativeId)
                {
                    return Result.Failure("Code is invalid for this cooperative");
                }
                if (coopCode is null)
                {
                    return Result.Failure("Invalid Code");
                }
                coopCode.IsUsed = true;
                _context.CooperativeUserCodes.Update(coopCode);
                await _context.SaveChangesAsync(cancellationToken);
                var result = new
                {
                    id = coopCode.CooperativeId,
                    code = coopCode.Code,
                    isUsed = coopCode.IsUsed,
                    email = coopCode.Email,
                    linkType = LinkType.Cooperative
                };
                return Result.Success("Verified", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Verification Failed:{ex.Message??ex.InnerException.Message}");
            }
        }
    }
}
