using MediatR;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.Users.Command.VerifyCooperativeInvitationCode
{
    public class VerifyCooperativeInvitationCodeCommand:IRequest<Result>
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }

    public class VerifyCooperativeInvitationCodeCommandHandler : IRequestHandler<VerifyCooperativeInvitationCodeCommand, Result>
    {
        
        public VerifyCooperativeInvitationCodeCommandHandler()
        {

        }
        public Task<Result> Handle(VerifyCooperativeInvitationCodeCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
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
