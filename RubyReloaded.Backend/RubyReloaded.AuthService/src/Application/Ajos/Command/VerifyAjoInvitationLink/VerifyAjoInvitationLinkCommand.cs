using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;

using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.Ajos.Command.VerifyAjoInvitationLink
{
    public class VerifyAjoInvitationLinkCommand : IRequest<Result>
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
    public class VerifyAjoInvitationLinkCommandHandler : IRequestHandler<VerifyAjoInvitationLinkCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        public VerifyAjoInvitationLinkCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(VerifyAjoInvitationLinkCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var decoded = ExtractToken(request.Token);
                var userid = decoded.Claims.First(claim => claim.Type == "userid").Value;
                var ajoId = Convert.ToInt32(decoded.Claims.First(claim => claim.Type == "sub").Value);
                var email = decoded.Claims.First(claim => claim.Type == "email").Value;
                var code = decoded.Claims.First(claim => claim.Type == "jti").Value;
                var userLink = await _context.UserLinkCreations.FirstOrDefaultAsync(x => x.Token == request.Token);
                var cooperative = await _context.Ajos.FirstOrDefaultAsync(x => x.Id == ajoId);

                if (userLink.IsUsed == true)
                {
                    return Result.Failure("This Link has been used");
                }
                if (code != cooperative.Code)
                {
                    return Result.Failure("Wrong Code");
                }
                if (userLink.RecipientEmail != request.Email)
                {
                    return Result.Failure("Wrong Email");
                }
                var cooperativeMembers = await _context.AjoMembers.Where(x => x.AjoId == ajoId).FirstOrDefaultAsync(x => x.UserId == userid);
                if (cooperativeMembers == null)
                {
                    return Result.Failure("User does not exist in this organization");
                }
                var result = new
                {
                    code,
                    email,
                    ajoId,

                };
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "verification failed", ex?.Message ?? ex?.InnerException.Message });
            }

        }
        internal static JwtSecurityToken ExtractToken(string str)
        {
            //var stream = str.Remove(0, 7);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(str);
            var token = jsonToken as JwtSecurityToken;
            return token;
        }
    }

}
