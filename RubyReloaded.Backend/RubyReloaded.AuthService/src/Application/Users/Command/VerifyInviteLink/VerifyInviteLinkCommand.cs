using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Ajos.Command.VerifyAjoInvitationCode;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Application.Users.Command.VerifyCooperativeInvitationCode;
using RubyReloaded.AuthService.Application.Users.Command.VerifyInvitationCode;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.User.Command.VerifyInviteLink
{
    public class VerifyInviteLinkCommand:IRequest<Result>
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }

    public class VerifyInviteLinkCommandHandler : IRequestHandler<VerifyInviteLinkCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        public VerifyInviteLinkCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(VerifyInviteLinkCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var decoded = ExtractToken(request.Token);
                var linktype = decoded.Claims.First(claim => claim.Type == "linktype").Value;
                var Id = Convert.ToInt32(decoded.Claims.First(claim => claim.Type == "sub").Value);
                var email = decoded.Claims.First(claim => claim.Type == "email").Value;
                var code = decoded.Claims.First(claim => claim.Type == "jti").Value;
               // var userLink = await _context.UserLinkCreations.FirstOrDefaultAsync(x => x.Token == request.Token);
               // var cooperative = await _context.Cooperatives.FirstOrDefaultAsync(x => x.Id == Id);

                if (linktype=="0")
                {
                    // run cooperative
                    var verifyCooperativeCodeCommand = new VerifyInvitationCodeCommand
                    {
                        Email = request.Email,
                        Code = code,
                        CooperativeId=Id
                    };
                    var handler = await new VerifyInvitationCodeCommandHandler(_context).Handle(verifyCooperativeCodeCommand, cancellationToken);
                    if (handler.Succeeded)
                    {
                        return Result.Success(handler.Entity);
                    }
                    return Result.Failure(handler.Message);
                }
                else
                {
                    //run ajo = code verification
                    var verifyAjoInvitationCodeCommand = new VerifyAjoInvitationCodeCommand
                    {
                        Code = code,
                        Email = email,
                        AjoId = Id
                    };
                    var handler = await new VerifyAjoInvitationCodeCommandHandler(_context).Handle(verifyAjoInvitationCodeCommand, cancellationToken);
                    if (handler.Succeeded)
                    {
                        return Result.Success(handler.Entity);
                    }
                    return Result.Failure(handler.Message);
                }



            
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
