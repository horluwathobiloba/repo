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

namespace RubyReloaded.AuthService.Application.Ajos.Command.VerifyAjoInvitationCode
{
    public class VerifyAjoInvitationCodeCommand : IRequest<Result>
    {
        public string Email { get; set; }
        public string Code { get; set; }
        public int AjoId { get; set; }
    }

    public class VerifyAjoInvitationCodeCommandHandler : IRequestHandler<VerifyAjoInvitationCodeCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        public VerifyAjoInvitationCodeCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(VerifyAjoInvitationCodeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var ajoCode = await _context.AjoCodes.FirstOrDefaultAsync(x => x.Code == request.Code && x.Email == request.Email);
                if (ajoCode.IsUsed)
                {
                    return Result.Failure("This code has already been used");
                }
                if (ajoCode.AjoId != request.AjoId)
                {
                    return Result.Failure("Code is invalid for this cooperative");
                }
                if (ajoCode is null)
                {
                    return Result.Failure("Invalid Code");
                }
                ajoCode.IsUsed = true;
                 _context.AjoCodes.Update(ajoCode);
                var result = new
                {
                    id = ajoCode.AjoId,
                    code = ajoCode.Code,
                    isUsed=ajoCode.IsUsed,
                    email=ajoCode.Email,
                    linkType=LinkType.Ajo
                };
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Verified", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Verification Failed:{ex.Message ?? ex.InnerException.Message}");
            }
        }
    }
}
